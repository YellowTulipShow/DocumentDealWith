using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using CodingSupportLibrary;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 计算可操作文件清单
    /// </summary>
    public class CalcCanOperationFileInventory
    {
        private readonly List<FileInfo> fileInventory;
        private readonly ILog log;
        private readonly DirectoryInfo rootDire;
        private readonly string[] allowExtensions;

        /// <summary>
        /// 实例化 - 计算可操作文件清单
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="rootDire">文件根目录</param>
        /// <param name="allowExtensions">允许文件扩展名</param>
        public CalcCanOperationFileInventory(ILog log, DirectoryInfo rootDire, string[] allowExtensions)
        {
            this.fileInventory = new List<FileInfo>();
            this.log = log;
            this.rootDire = rootDire;
            this.allowExtensions = allowExtensions;
            OInit();
        }

        private void OInit()
        {
            if (allowExtensions == null || allowExtensions.Length <= 0)
            {
                throw new ArgumentNullException(nameof(allowExtensions), "允许扩展名为空");
            }
            for (int i = 0; i < allowExtensions.Length; i++)
            {
                allowExtensions[i] = allowExtensions[i]?.Trim()?.ToLower() ?? string.Empty;
            }
        }

        /// <summary>
        /// 添加操作文件清单
        /// </summary>
        /// <param name="filePaths">文件路径字符串清单</param>
        public void Append(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < filePaths.Length; i++)
            {
                var filePath = filePaths[i];
                var file = ToFile(filePath);
                if (file != null && allowExtensions.Contains(file.Extension.Trim().ToLower()))
                {
                    fileInventory.Add(file);
                }
            }
        }

        private FileInfo ToFile(string path)
        {
            path = path?.Trim();
            string root = rootDire?.FullName?.Trim();
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(root))
                return null;
            FileInfo info;
            if (Path.IsPathRooted(path))
            {
                info = new FileInfo(path);
            }
            else
            {
                path = Path.Combine(root, path);
                info = new FileInfo(path);
            }
            DirectoryInfo dire = info.Directory;
            if (!dire.Exists)
            {
                dire.Create();
            }
            return info;
        }

        /// <summary>
        /// 添加操作文件清单
        /// </summary>
        /// <param name="filePaths">文件队列</param>
        public void Append(FileInfo[] filePaths)
        {
            if (filePaths == null || filePaths.Length <= 0)
                return;
            Append(filePaths.Select(b => b.FullName).ToArray());
        }

        /// <summary>
        /// 添加操作文件清单
        /// </summary>
        /// <param name="direPath">目录地址</param>
        /// <param name="direPathIsRecurse">是否递归查询</param>
        public void Append(DirectoryInfo direPath, bool direPathIsRecurse)
        {
            if (direPath == null)
            {
                return;
            }
            FileInfo[] filePaths = direPath.GetFiles();
            Append(filePaths);
            if (direPathIsRecurse)
            {
                DirectoryInfo[] subDires = direPath.GetDirectories();
                for (int i = 0; i < subDires.Length; i++)
                {
                    DirectoryInfo subDire = subDires[i];
                    Append(subDire, direPathIsRecurse);
                }
            }
        }

        /// <summary>
        /// 添加操作文件清单
        /// </summary>
        /// <param name="fileText">存放文件路径文档地址</param>
        public void Append(FileInfo fileText)
        {
            if (fileText == null || !fileText.Exists)
            {
                return;
            }
            Encoding fileEncoding = fileText.GetEncoding();
            if (fileEncoding == null)
                throw new FileLoadException("无法识别清单文件的编码类型");
            string[] liens = File.ReadAllLines(fileText.FullName, fileEncoding);
            liens = liens.Where(b => !string.IsNullOrEmpty(b)).ToArray();
            Append(liens);
        }

        /// <summary>
        /// 获取文件清单队列
        /// </summary>
        public FileInfo[] GetResults()
        {
            return fileInventory.ToArray();
        }
    }
}
