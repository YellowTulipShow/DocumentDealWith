using System.Linq;
using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public abstract class AbsCommandParamConfig_Rename<P> : AbsBasicParamConfig<P> where P : AbsParamRename, new()
    {
        /// <inheritdoc/>
        public AbsCommandParamConfig_Rename(YTS.Log.ILog log) : base(log)
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

        /// <inheritdoc/>
        public override string[] GetConfigAllowExtensions(Configs config)
        {
            return base.GetConfigAllowExtensions(config)
                .ConcatCanNull(config.AllowExtension.RenameCommand)
                .ToArray();
        }
    }
}
