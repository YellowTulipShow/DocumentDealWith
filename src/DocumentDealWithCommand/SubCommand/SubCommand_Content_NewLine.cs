using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.ParamConfigs;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 换行符
    /// </summary>
    public class SubCommand_Content_NewLine : ICommandSub<CommandParameters_Content_NewLine>
    {
        private readonly ILog log;

        /// <inheritdoc/>
        public SubCommand_Content_NewLine(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public string GetNameSign() => "newline";

        /// <inheritdoc/>
        public string GetDescription() => "重新配置换行符";

        /// <inheritdoc/>
        public IExecute<CommandParameters_Content_NewLine> GetExecute()
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
