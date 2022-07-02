using System.Linq;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

using YTS.Log;

using CodingSupportLibrary;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 编码修改
    /// </summary>
    public class SubCommand_Content_Encoding : AbsSubCommandImplementationVersion<CommandParameters_Content_Encoding>, ISubCommand
    {
        private readonly Option<ESupportEncoding> Option_TargetEncoding;

        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
            Option_TargetEncoding = GetOption_TargetEncoding();
        }

        private Option<ESupportEncoding> GetOption_TargetEncoding()
        {
            var option = new Option<ESupportEncoding>(
                aliases: new string[] { "-t", "--target" },
                description: "目标编码配置")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }

        /// <inheritdoc/>
        public override string CommandNameSign() => "encode";

        /// <inheritdoc/>
        public override string CommandDescription() => "重新配置编码";

        /// <inheritdoc/>
        public override IEnumerable<ISubCommand> SetSubCommands() => null;

        /// <inheritdoc/>
        public override IMain<CommandParameters_Content_Encoding> HandlerLogic()
        {
            return new Main_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<Option> SetOptions()
        {
            yield return Option_TargetEncoding;
        }

        /// <inheritdoc/>
        public override CommandParameters_Content_Encoding FillParam(InvocationContext context, CommandParameters_Content_Encoding param)
        {
            param.Target = context.ParseResult.GetValueForOption(Option_TargetEncoding);
            return param;
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.ContentCommand ?? new string[] { }).ToArray();
        }
    }
}
