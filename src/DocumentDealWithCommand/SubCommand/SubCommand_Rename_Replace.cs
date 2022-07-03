using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 替换
    /// </summary>
    public class SubCommand_Rename_Replace : AbsSubCommandImplementationVersion<CommandParameters_Rename_Replace>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename_Replace(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "rename";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Rename_Replace> HandlerLogic()
        {
            // return new Main_Rename(log);
            return null;
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return SubCommand_Rename.CalcAllowExtensions(config);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<CommandParameters_Rename_Replace>> SetOptions()
        {
            yield return new OptionRegistration<bool, CommandParameters_Rename_Replace>
                (GetOption_IsPreview(), (param, value) => param.IsPreview = value);

            yield return new OptionRegistration<string, CommandParameters_Rename_Replace>
                (GetOption_Pattern(), (param, value) => param.Pattern = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_Replace>
                (GetOption_Replacement(), (param, value) => param.Replacement = value);
        }
        private Option<bool> GetOption_IsPreview()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--is-preview" },
                description: "是否预览",
                getDefaultValue: () => true)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<string> GetOption_Pattern()
        {
            var option = new Option<string>(
                aliases: new string[] { "--Pattern" },
                description: "匹配项, 支持正则表达式")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_Replacement()
        {
            var option = new Option<string>(
                aliases: new string[] { "--Replacement" },
                description: "替换内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
