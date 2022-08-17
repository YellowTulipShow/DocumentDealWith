using System.CommandLine;
using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;
using CommandParamUse.Implementation;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Content_NewLine : AddParamConfigDefalutValue<CommandParameters_Content_NewLine>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Content_NewLine() : base()
        {
            new Option<ENewLineType>(
                aliases: new string[] { "-t", "--type" },
                description: "目标换行标识")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }.Set(this, (param, value) => param.Type = value);
        }
    }

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
            return new SubCommandParamConfig_Content_NewLine();
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
