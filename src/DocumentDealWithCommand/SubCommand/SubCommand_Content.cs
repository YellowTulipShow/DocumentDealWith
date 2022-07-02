using System.Linq;
using System.CommandLine;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;
using DocumentDealWithCommand.Logic;
using System.Collections.Generic;
using System.CommandLine.Invocation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容相关操作
    /// </summary>
    public class SubCommand_Content : AbsSubCommandImplementationVersion<BasicCommandParameters>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override string CommandNameSign() => "content";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IEnumerable<ISubCommand> SetSubCommands()
        {
            yield return new SubCommand_Content_Encoding(log, globalOptions);
            yield return new SubCommand_Content_NewLine(log, globalOptions);
        }

        /// <inheritdoc/>
        public override IMain<BasicCommandParameters> HandlerLogic() => null;

        /// <inheritdoc/>
        public override IEnumerable<Option> SetOptions() => null;

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.ContentCommand ?? new string[] { }).ToArray();
        }

        /// <inheritdoc/>
        public override BasicCommandParameters FillParam(InvocationContext context, BasicCommandParameters param) => param;
    }
}
