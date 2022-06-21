using System.IO;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 抽象类: 基础名称传入参数
    /// </summary>
    public class AbsBasicCommandParameters
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public Configs Config { get; set; }

        /// <summary>
        /// 需操作文件
        /// </summary>
        public FileInfo[] Files { get; set; }

        /// <summary>
        /// 需操作目录
        /// </summary>
        public DirectoryInfo Path { get; set; }

        /// <summary>
        /// 需操作目录, 是否查询递归
        /// </summary>
        public bool PathIsRecurse { get; set; }

        /// <summary>
        /// 需操作文件路径清单文件路径
        /// </summary>
        public FileInfo FileText { get; set; }
    }
}
