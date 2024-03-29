﻿using System.IO;
using System.Text.RegularExpressions;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 文件信息相关扩展操作方法
    /// </summary>
    public static class FileInfoExtend
    {
        /// <summary>
        /// 展示输出文件名称
        /// </summary>
        /// <param name="info">文件信息</param>
        /// <param name="rootDire">预期的文件根目录</param>
        /// <returns>z展示用的文件路径</returns>
        public static string ToShowFileName(this FileInfo info, DirectoryInfo rootDire = null)
        {
            string rootDirePath = rootDire
                ?.FullName
                ?.Trim('\\')
                ?.Trim('/') ?? string.Empty;
            string name = info.FullName;
            if (!string.IsNullOrEmpty(rootDirePath))
                name = name.Replace(rootDirePath, "~");
            name = name
                .Replace('\\', '/')
                .Replace(":", "")
                .Trim('\\')
                .Trim('/');
            if (Regex.IsMatch(name, @"^[a-z]/", RegexOptions.ECMAScript | RegexOptions.IgnoreCase))
                return $"/{name}";
            return name;
        }

        /// <summary>
        /// 转换为文件名称部分获取信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>文件信息</returns>
        public static MFileName ToMFileName(string fileName)
        {
            MFileName m = new MFileName()
            {
                Name = string.Empty,
                Extension = string.Empty,
            };
            if (string.IsNullOrEmpty(fileName))
            {
                return m;
            }
            if (Regex.IsMatch(fileName, @"^[a-z0-9]+$"))
            {
                m.Name = fileName;
                return m;
            }
            if (Regex.IsMatch(fileName, @"^\.[a-z0-9]+$"))
            {
                m.Extension = fileName;
                return m;
            }
            Match match = Regex.Match(fileName, @"([^\\/]*)(\.[a-z0-9]*)$",
                RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            if (match.Success)
            {
                m.Name = match.Groups[1].Value;
                m.Extension = match.Groups[2].Value;
                return m;
            }
            return m;
        }
    }
}
