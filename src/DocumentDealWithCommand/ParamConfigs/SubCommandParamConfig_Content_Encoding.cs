using System.CommandLine;

using YTS.CodingSupportLibrary;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Content_Encoding : AbsCommandParamConfig_Content<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Content_Encoding(YTS.Log.ILog log) : base(log)
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
