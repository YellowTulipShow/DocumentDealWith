using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;

using YTS.Log;

namespace CommandParamUse
{
    /// <summary>
    /// 抽象类: 基础命令
    /// </summary>
    public abstract class AbsCommand
    {
        private readonly ILog log;
        private readonly ICommand parent;

        /// <summary>
        /// 实例化基础命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="parent">父级命令</param>
        public AbsCommand(ILog log, ICommand parent)
        {
        }
    }
}
