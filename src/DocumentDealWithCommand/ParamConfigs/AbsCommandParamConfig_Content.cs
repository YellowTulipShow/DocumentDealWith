using System.Linq;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public abstract class AbsCommandParamConfig_Content<P> : AbsBasicParamConfig<P> where P : BasicCommandParameters, new()
    {
        /// <inheritdoc/>
        public AbsCommandParamConfig_Content(YTS.Log.ILog log) : base(log) { }

        /// <inheritdoc/>
        public override string[] GetConfigAllowExtensions(Configs config)
        {
            return base.GetConfigAllowExtensions(config)
                .ConcatCanNull(config.AllowExtension.ContentCommand)
                .ToArray();
        }
    }
}
