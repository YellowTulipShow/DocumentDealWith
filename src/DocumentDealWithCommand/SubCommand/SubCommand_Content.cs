using System.Linq;
using System.Collections.Generic;

using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容相关操作
    /// </summary>
    public class SubCommand_Content : AbsSubCommandImplementationVersion<BasicCommandParameters>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

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
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return CalcAllowExtensions(config);
        }

        /// <summary>
        /// 取公共配置+内容配置
        /// </summary>
        public static string[] CalcAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.ContentCommand ?? new string[] { }).ToArray();
        }
    }
}
