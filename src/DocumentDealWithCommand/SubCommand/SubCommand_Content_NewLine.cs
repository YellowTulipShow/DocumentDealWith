using System;
using System.CommandLine;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

using YTS.Log;
using System.CommandLine.Invocation;
using System.Collections.Generic;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 换行符
    /// </summary>
    public class SubCommand_Content_NewLine : AbsSubCommandImplementationVersion<CommandParameters_Content_NewLine>, ISubCommand
    {
        private readonly Option<ENewLineType> Option_Type;
        /// <inheritdoc/>
        public SubCommand_Content_NewLine(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
            Option_Type = GetOption_Type();
        }

        private Option<ENewLineType> GetOption_Type()
        {
            var option = new Option<ENewLineType>(
                aliases: new string[] { "-t", "--type" },
                description: "目标换行标识")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }

        /// <inheritdoc/>
        public override string CommandNameSign() => "newline";

        /// <inheritdoc/>
        public override string CommandDescription() => "重新配置换行符";

        /// <inheritdoc/>
        public override IEnumerable<ISubCommand> SetSubCommands() => null;

        /// <inheritdoc/>
        public override IMain<CommandParameters_Content_NewLine> HandlerLogic()
        {
            return new Main_Content_NewLine(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<Option> SetOptions()
        {
            yield return Option_Type;
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override CommandParameters_Content_NewLine FillParam(InvocationContext context, CommandParameters_Content_NewLine param)
        {
            param.Type = context.ParseResult.GetValueForOption(Option_Type);
            return param;
        }
    }
}
