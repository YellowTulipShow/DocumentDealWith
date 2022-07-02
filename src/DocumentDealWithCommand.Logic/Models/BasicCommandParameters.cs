using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

using YTS.ConsolePrint;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 抽象类: 基础名称传入参数
    /// </summary>
    public class BasicCommandParameters : ICommandParameters
    {
        /// <summary>
        /// 实例化参数
        /// </summary>
        public BasicCommandParameters() { }

        /// <summary>
        /// 配置信息
        /// </summary>
        public Configs Config { get; set; }

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

        /// <inheritdoc/>
        public virtual void WriteLogArgs(IDictionary<string, object> logArgs)
        {
            logArgs["Config"] = JsonConvert.SerializeObject(Config);
            logArgs["NeedHandleFileInventory.Length"] = NeedHandleFileInventory.Length;
            logArgs["ConsoleType"] = ConsoleType.ToString();
            logArgs["Print.GetType().FullName"] = Print.GetType().FullName;
        }
    }
}
