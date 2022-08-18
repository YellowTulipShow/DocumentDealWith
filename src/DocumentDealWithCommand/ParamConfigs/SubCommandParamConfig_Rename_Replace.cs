using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename_Replace : SubCommandParamConfig_Rename<ParamRenameReplace>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename_Replace() : base()
        {
            new Option<string>(
                aliases: new string[] { "--pattern" },
                description: "匹配项, 支持正则表达式")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }
            .Set(this, (param, value) => param.Pattern = value);

            new Option<string>(
                aliases: new string[] { "--replacement" },
                description: "替换内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }
            .Set(this, (param, value) => param.Replacement = value);
        }
    }
}
