using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text;

using YTS.ConsolePrint;
using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.SubCommand;
using DocumentDealWithCommand.Logic;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 主命令
    /// </summary>
    public class MainCommand : AbsUseLog, ICommandRoot
    {
        /// <summary>
        /// 实例化: 主命令
        /// </summary>
        /// <param name="log">日志接口</param>
        public MainCommand(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetDescription() => "文档文件相关操作命令";

        /// <inheritdoc/>
        public virtual IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new SubCommand_Content(log);
            yield return new SubCommand_Rename_Whole(log);
        }
    }
}
