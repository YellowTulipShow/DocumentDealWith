using System.CommandLine;
using System.Collections.Generic;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 换行符
    /// </summary>
    public class SubCommand_Content_NewLine : AbsSubCommandImplementationVersion<CommandParameters_Content_NewLine>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content_NewLine(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "newline";

        /// <inheritdoc/>
        public override string CommandDescription() => "重新配置换行符";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Content_NewLine> HandlerLogic()
        {
            return new Main_Content_NewLine(log);
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return SubCommand_Content.CalcAllowExtensions(config);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<CommandParameters_Content_NewLine>> SetOptions()
        {
            yield return new OptionRegistration<ENewLineType, CommandParameters_Content_NewLine>(
                new Option<ENewLineType>(
                aliases: new string[] { "-t", "--type" },
                description: "目标换行标识")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = true,
                },
                (param, value) => param.Type = value);
        }
    }
}
