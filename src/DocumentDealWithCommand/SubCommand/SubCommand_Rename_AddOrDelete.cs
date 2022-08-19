using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.ParamConfigs;
using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 添加或删除
    /// </summary>
    public class SubCommand_Rename_AddOrDelete : AbsUseLog, ICommandSub<ParamRenameAddOrDelete>
    {
        /// <inheritdoc/>
        public SubCommand_Rename_AddOrDelete(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "ad";

        /// <inheritdoc/>
        public string GetDescription() => "添加或删除";

        /// <inheritdoc/>
        public virtual IExecute<ParamRenameAddOrDelete> GetExecute()
        {
            return new Main_Rename_AddOrDelete(log);
        }

        /// <inheritdoc/>
        public IParamConfig<ParamRenameAddOrDelete> GetParamConfig()
        {
            return new SubCommandParamConfig_Rename_AddOrDelete(log);
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
