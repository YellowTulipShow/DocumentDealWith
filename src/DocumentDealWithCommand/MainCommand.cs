using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text;

using YTS.ConsolePrint;
using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.SubCommand;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 主命令
    /// </summary>
    public class MainCommand : ICommandRoot
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化主命令
        /// </summary>
        /// <param name="log"></param>
        public MainCommand(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public string GetDescription()
        {
            return "文档文件相关操作命令";
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new SubCommand_Content(log);
        }
    }
}
