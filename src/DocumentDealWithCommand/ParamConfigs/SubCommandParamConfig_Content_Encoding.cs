using System.CommandLine;

using YTS.CodingSupportLibrary;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Content_Encoding : AbsBasicParamConfig<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Content_Encoding() : base()
        {
            new Option<ESupportEncoding>(
                aliases: new string[] { "-t", "--target" },
                description: "目标编码配置")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }.Set(this, (param, value) => param.Target = value);
        }
    }
}
