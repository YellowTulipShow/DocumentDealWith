using System.IO;

using YTS.ConsolePrint;

using CommandParamUse;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 抽象类: 基础名称传入参数
    /// </summary>
    public class BasicCommandParameters : IParam
    {
        /// <summary>
        /// 全局参数
        /// </summary>
        public readonly GlobalOptionsValue GlobalOptions;

        /// <summary>
        /// 实例化参数
        /// </summary>
        public BasicCommandParameters()
        {
            GlobalOptions = new GlobalOptionsValue();
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public Configs Config { get; set; }

        /// <summary>
        /// 根目录
        /// </summary>
        public DirectoryInfo RootDire { get; set; }

        /// <summary>
        /// 需要处理的文件清单
        /// </summary>
        public FileInfo[] NeedHandleFileInventory { get; set; }

        /// <summary>
        /// 输出打印配置, 控制台类型
        /// </summary>
        public EConsoleType ConsoleType { get; set; }

        /// <summary>
        /// 控制台输出打印接口
        /// </summary>
        public IPrintColor Print { get; set; }
    }
}
