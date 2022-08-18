using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Content_NewLine : MainCommandParamConfig<CommandParameters_Content_NewLine>
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
}
