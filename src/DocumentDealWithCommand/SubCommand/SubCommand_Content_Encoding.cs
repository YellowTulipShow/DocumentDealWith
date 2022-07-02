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
        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "encode";

        /// <inheritdoc/>
        public override string CommandDescription() => "重新配置编码";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Content_Encoding> HandlerLogic()
        {
            return new Main_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<CommandParameters_Content_Encoding>> SetOptions()
        {
            yield return new OptionRegistration<ESupportEncoding, CommandParameters_Content_Encoding>
                (GetOption_TargetEncoding(), (param, value) => param.Target = value);
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
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return SubCommand_Content.CalcAllowExtensions(config);
        }
    }
}
