using YTS.ConsolePrint;

using CommandParamUse;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 全局选项集合
    /// </summary>
    public struct GlobalOptionsValue : IParam
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 操作文件的根目录, 否则默认是执行目录
        /// </summary>
        public string RootDire { get; set; }

        /// <summary>
        /// 输出打印配置, 控制台类型
        /// </summary>
        public EConsoleType ConsoleType { get; set; }

        /// <summary>
        /// 需操作文件
        /// </summary>
        public string[] Files { get; set; }

        /// <summary>
        /// 需操作目录
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 需操作目录, 是否查询递归
        /// </summary>
        public bool PathIsRecurse { get; set; }

        /// <summary>
        /// 需操作文件路径清单文件路径
        /// </summary>
        public string FileText { get; set; }
    }
}
