using System.IO;
using System.Text.RegularExpressions;

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
                name = name.Replace(rootDirePath, ".");
            name = name
                .Replace('\\', '/')
                .Replace(":", "")
                .Trim('\\')
                .Trim('/');
            if (Regex.IsMatch(name, @"^[a-z]/", RegexOptions.ECMAScript | RegexOptions.IgnoreCase))
                return $"/{name}";
            return name;
        }
    }
}
