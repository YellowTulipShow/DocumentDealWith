using System;
using System.CommandLine;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名
    /// </summary>
    public class SubCommand_Rename : AbsSubCommand, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
