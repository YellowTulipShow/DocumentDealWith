using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename<P> : MainCommandParamConfig<P> where P : AbsParamRename, new()
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename() : base()
        {
            new Option<bool>(
                aliases: new string[] { "--is-preview" },
                description: "是否预览",
                getDefaultValue: () => true)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }.SetGlobal(this, (param, value) => param.IsPreview = value);

            new Option<uint>(
                aliases: new string[] { "--preview-column" },
                description: "预览展示列数量",
                getDefaultValue: () => 0)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }.SetGlobal(this, (param, value) => param.PreviewColumnCount = value);
        }
    }
}
