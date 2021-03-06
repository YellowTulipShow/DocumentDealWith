using System;
using System.IO;
using System.Linq;

using DocumentDealWithCommand.Logic.Models;

using YTS.ConsolePrint;
using YTS.Log;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 抽象类: 执行逻辑
    /// </summary>
    public abstract class AbsMain
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;

        /// <summary>
        /// 初始化实例执行逻辑
        /// </summary>
        /// <param name="log">日志接口</param>
        public AbsMain(ILog log)
        {
            this.log = log;
        }
    }
}
