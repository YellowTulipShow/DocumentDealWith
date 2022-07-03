using System;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名
    /// </summary>
    public class SubCommand_Rename : AbsSubCommandImplementationVersion<CommandParameters_Rename>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "rename";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Rename> HandlerLogic()
        {
            return new Main_Rename(log);
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return CalcAllowExtensions(config);
        }

        /// <summary>
        /// 取公共配置+重命名配置
        /// </summary>
        public static string[] CalcAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.RenameCommand ?? new string[] { }).ToArray();
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<CommandParameters_Rename>> SetOptions()
        {
            yield return new OptionRegistration<bool, CommandParameters_Rename>
                (GetOption_IsPreview(), (param, value) => param.IsPreview = value);

            yield return new OptionRegistration<string, CommandParameters_Rename>
                (GetOption_NamingRules(), (param, value) => param.NamingRules = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename>
                (GetOption_StartedOnIndex(), (param, value) => param.StartedOnIndex = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename>
                (GetOption_Increment(), (param, value) => param.Increment = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename>
                (GetOption_Digit(), (param, value) => param.Digit = value);
            yield return new OptionRegistration<bool, CommandParameters_Rename>
                (GetOption_IsInsufficientSupplementBit(), (param, value) => param.IsInsufficientSupplementBit = value);
            yield return new OptionRegistration<bool, CommandParameters_Rename>
                (GetOption_IsUseLetter(), (param, value) => param.IsUseLetter = value);
            yield return new OptionRegistration<ERenameLetterFormat, CommandParameters_Rename>
                (GetOption_UseLetterFormat(), (param, value) => param.UseLetterFormat = value);
            yield return new OptionRegistration<bool, CommandParameters_Rename>
                (GetOption_IsChangeExtension(), (param, value) => param.IsChangeExtension = value);
            yield return new OptionRegistration<string, CommandParameters_Rename>
                (GetOption_ChangeExtensionValue(), (param, value) => param.ChangeExtensionValue = value);
            yield return new OptionRegistration<bool, CommandParameters_Rename>
                (GetOption_IsAutomaticallyResolveRenameConflicts(), (param, value) => param.IsAutomaticallyResolveRenameConflicts = value);
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
        private Option<string> GetOption_NamingRules()
        {
            var option = new Option<string>(
                aliases: new string[] { "--NamingRules" },
                description: "命名规则")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_StartedOnIndex()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--StartedOnIndex" },
                description: "开始于",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_Increment()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--Increment" },
                description: "增量",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_Digit()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--Digit" },
                description: "位数",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<bool> GetOption_IsInsufficientSupplementBit()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--IsInsufficientSupplementBit" },
                description: "是否不足位补齐",
                getDefaultValue: () => true)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<bool> GetOption_IsUseLetter()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--IsUseLetter" },
                description: "是否使用字母",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<ERenameLetterFormat> GetOption_UseLetterFormat()
        {
            var option = new Option<ERenameLetterFormat>(
                aliases: new string[] { "--UseLetterFormat" },
                description: "使用字母格式",
                getDefaultValue: () => ERenameLetterFormat.Lower)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<bool> GetOption_IsChangeExtension()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--IsChangeExtension" },
                description: "是否更改扩展名",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<string> GetOption_ChangeExtensionValue()
        {
            var option = new Option<string>(
                aliases: new string[] { "--ChangeExtensionValue" },
                description: "更改扩展名内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
        private Option<bool> GetOption_IsAutomaticallyResolveRenameConflicts()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--IsAutomaticallyResolveRenameConflicts" },
                description: "是否自动解决重命名冲突",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            };
            return option;
        }
    }
}
