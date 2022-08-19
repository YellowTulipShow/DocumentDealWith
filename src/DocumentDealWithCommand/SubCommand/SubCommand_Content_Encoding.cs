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
    /// 子命令: 内容 - 编码修改
    /// </summary>
    public class SubCommand_Content_Encoding : AbsUseLog, ICommandSub<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "encode";

        /// <inheritdoc/>
        public string GetDescription() => "重新配置编码";

        /// <inheritdoc/>
        public virtual IExecute<CommandParameters_Content_Encoding> GetExecute()
        {
            return new Main_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public IParamConfig<CommandParameters_Content_Encoding> GetParamConfig()
        {
            return new SubCommandParamConfig_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
