using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 更改文件名称实现功能类
    /// </summary>
    public class ChangeFileNameHelp
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化更改文件名称实现功能类
        /// </summary>
        /// <param name="log">日志接口</param>
        public ChangeFileNameHelp(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 执行重命名
        /// </summary>
        /// <param name="print">打印输出接口</param>
        /// <param name="rootDire">根目录</param>
        /// <param name="rlist">更改文件名称计算接口队列</param>
        /// <returns>执行结果编号</returns>
        public int ChangeFileName(IPrintColor print, DirectoryInfo rootDire, IList<HandleRenameResult> rlist)
        {
            var logArgs = log.CreateArgDictionary();
            IDictionary<string, HandleRenameResult> temporaryNameDict = new Dictionary<string, HandleRenameResult>();
            const string logArgsNameExecuteRegion = "执行部分";
            logArgs[logArgsNameExecuteRegion] = "非临时文件相关重命名";
            for (int i = 0; i < rlist.Count; i++)
            {
                try
                {
                    var m = rlist[i];
                    logArgs["m.Source.FullName"] = m.Source.FullName;
                    logArgs["m.Result"] = m.Result;
                    DirectoryInfo dire = m.Source.Directory;
                    string targetFilePath = Path.Combine(dire.FullName, m.Result);
                    FileInfo targetFile;
                    if (File.Exists(targetFilePath))
                    {
                        FileInfo temporaryFile = CalcTemporaryName(m.Source, i);
                        string temporaryFilePath = temporaryFile.FullName;
                        temporaryNameDict[temporaryFilePath] = m;
                        targetFile = temporaryFile;
                    }
                    else
                    {
                        targetFile = new FileInfo(targetFilePath);
                    }
                    Move(print, m.Source, targetFile, rootDire);
                }
                catch (Exception ex)
                {
                    log.Error($"{logArgs[logArgsNameExecuteRegion]}执行出错", ex, logArgs);
                    continue;
                }
            }
            logArgs[logArgsNameExecuteRegion] = "临时文件相关重命名";
            foreach (string temporaryNamePath in temporaryNameDict.Keys)
            {
                try
                {
                    var m = temporaryNameDict[temporaryNamePath];
                    logArgs["m.Source.FullName"] = m.Source.FullName;
                    logArgs["m.Result"] = m.Result;
                    DirectoryInfo dire = m.Source.Directory;
                    string newFilePath;
                    do
                    {
                        newFilePath = Path.Combine(dire.FullName, m.Result);
                        if (!File.Exists(newFilePath))
                            break;
                        m.Result = Regex.Replace(m.Result, @"^(.*)\.([a-z0-9])$", @"$1_Repeat.$2");
                        logArgs["m.Result"] = m.Result;
                    } while (true);
                    FileInfo temporaryFile = new FileInfo(temporaryNamePath);
                    FileInfo newFile = new FileInfo(newFilePath);
                    Move(print, temporaryFile, newFile, rootDire);
                }
                catch (Exception ex)
                {
                    log.Error($"{logArgs[logArgsNameExecuteRegion]}执行出错", ex, logArgs);
                    continue;
                }
            }
            return 0;
        }
        private FileInfo CalcTemporaryName(FileInfo info, int index)
        {
            string name = $".t.n.{index}";
            do
            {
                string path = Path.Combine(info.Directory.FullName, $"{name}{info.Extension}");
                if (!File.Exists(path))
                {
                    return new FileInfo(path);
                }
                name += ".c";
            } while (true);
        }
        private void Move(IPrintColor print, FileInfo sourceFile, FileInfo targetFile, DirectoryInfo rootDire)
        {
            string show_name = sourceFile.ToShowFileName(rootDire);
            sourceFile.MoveTo(targetFile.FullName);
            print.WriteLine($"重命名: {show_name} => {targetFile.Name}");
        }
    }
}
