using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.ParamConfigs;
using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名
    /// </summary>
    public class SubCommand_Rename_Whole : AbsUseLog, ICommandSub<ParamRenameWhole>
    {
        /// <inheritdoc/>
        public SubCommand_Rename_Whole(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "rename";

        /// <inheritdoc/>
        public string GetDescription() => "重命名操作";

        /// <inheritdoc/>
        public virtual IExecute<ParamRenameWhole> GetExecute()
        {
            return new Main_Rename_Whole(log);
        }

        /// <inheritdoc/>
        public IParamConfig<ParamRenameWhole> GetParamConfig()
        {
            return new SubCommandParamConfig_Rename_Whole(log);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new SubCommand_Rename_Replace(log);
            yield return new SubCommand_Rename_AddOrDelete(log);
        }
    }
}
