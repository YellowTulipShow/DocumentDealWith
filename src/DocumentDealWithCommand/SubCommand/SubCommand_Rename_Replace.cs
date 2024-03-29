﻿using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.ParamConfigs;
using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 替换
    /// </summary>
    public class SubCommand_Rename_Replace : AbsUseLog, ICommandSub<ParamRenameReplace>
    {
        /// <inheritdoc/>
        public SubCommand_Rename_Replace(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "replace";

        /// <inheritdoc/>
        public string GetDescription() => "替换";

        /// <inheritdoc/>
        public virtual IExecute<ParamRenameReplace> GetExecute()
        {
            return new Main_Rename_Replace(log);
        }

        /// <inheritdoc/>
        public IParamConfig<ParamRenameReplace> GetParamConfig()
        {
            return new SubCommandParamConfig_Rename_Replace(log);
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
