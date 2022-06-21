﻿using System;
using System.CommandLine;
using System.Linq;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容相关操作
    /// </summary>
    public class SubCommand_Content : AbsSubCommand, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Command cmd = new Command("content", "内容操作");
            ISubCommand[] subCommands = new ISubCommand[]
            {
                new SubCommand_Content(log, globalOptions),
                new SubCommand_Rename(log, globalOptions),
            };
            foreach (var sub in subCommands)
            {
                Command subCMD = sub.GetCommand();
                cmd.AddCommand(subCMD);
            }
            return cmd;
        }
    }
}
