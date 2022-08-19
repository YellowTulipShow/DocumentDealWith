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
    /// 子命令: 内容 - 换行符
    /// </summary>
    public class SubCommand_Content_NewLine : AbsUseLog, ICommandSub<CommandParameters_Content_NewLine>
    {
        /// <inheritdoc/>
        public SubCommand_Content_NewLine(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "newline";

        /// <inheritdoc/>
        public string GetDescription() => "重新配置换行符";

        /// <inheritdoc/>
        public virtual IExecute<CommandParameters_Content_NewLine> GetExecute()
        {
            return new Main_Content_NewLine(log);
        }

        /// <inheritdoc/>
        public IParamConfig<CommandParameters_Content_NewLine> GetParamConfig()
        {
            return new SubCommandParamConfig_Content_NewLine(log);
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
