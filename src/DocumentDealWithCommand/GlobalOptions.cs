using System.IO;
using System.CommandLine;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 全局选项集合
    /// </summary>
    public struct GlobalOptions
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public Option<string> Config { get; set; }

        /// <summary>
        /// 需操作文件
        /// </summary>
        public Option<string[]> Files { get; set; }

        /// <summary>
        /// 需操作目录
        /// </summary>
        public Option<DirectoryInfo> Path { get; set; }

        /// <summary>
        /// 需操作目录, 是否查询递归
        /// </summary>
        public Option<bool> PathIsRecurse { get; set; }

        /// <summary>
        /// 需操作文件路径清单文件路径
        /// </summary>
        public Option<FileInfo> FileText { get; set; }
    }
}
