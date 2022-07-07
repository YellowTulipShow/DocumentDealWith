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
    public class SubCommand_Rename_Whole : AbsSubCommand_Rename<ParamRenameWhole>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename_Whole(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "rename";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IMain<ParamRenameWhole> HandlerLogic()
        {
            return new Main_Rename_Whole(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<ISubCommand> SetSubCommands()
        {
            yield return new SubCommand_Rename_Replace(log, globalOptions);
            yield return new SubCommand_Rename_AddOrDelete(log, globalOptions);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<ParamRenameWhole>> SetOptions()
        {
            var base_list = base.SetOptions();
            if (base_list != null)
            {
                foreach (var base_item in base_list)
                {
                    yield return base_item;
                }
            }

            yield return new OptionRegistration<string, ParamRenameWhole>(
                new Option<string>(
                aliases: new string[] { "--rule" },
                description: "命名规则")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = true,
                },
                (param, value) => param.NamingRules = value);
            yield return new OptionRegistration<int, ParamRenameWhole>(
                new Option<int>(
                aliases: new string[] { "--start" },
                description: "开始于",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.StartedOnIndex = value);
            yield return new OptionRegistration<int, ParamRenameWhole>(
                new Option<int>(
                aliases: new string[] { "--increment" },
                description: "增量",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.Increment = value);
            yield return new OptionRegistration<uint, ParamRenameWhole>(
                new Option<uint>(
                aliases: new string[] { "--digit" },
                description: "位数",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.Digit = value);

            yield return new OptionRegistration<bool, ParamRenameWhole>(
                new Option<bool>(
                aliases: new string[] { "--is-repair-bit" },
                description: "是否不足位补齐",
                getDefaultValue: () => true)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsInsufficientSupplementBit = value);

            yield return new OptionRegistration<bool, ParamRenameWhole>(
                new Option<bool>(
                aliases: new string[] { "--is-use-letter" },
                description: "是否使用字母",
                getDefaultValue: () => false)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsUseLetter = value);

            yield return new OptionRegistration<ERenameLetterFormat, ParamRenameWhole>(
                new Option<ERenameLetterFormat>(
                aliases: new string[] { "--use-letter-format" },
                description: "使用字母类型格式",
                getDefaultValue: () => ERenameLetterFormat.Lower)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.UseLetterFormat = value);

            yield return new OptionRegistration<bool, ParamRenameWhole>(
                new Option<bool>(
                aliases: new string[] { "--is-change-extension" },
                description: "是否更改扩展名",
                getDefaultValue: () => false)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsChangeExtension = value);

            yield return new OptionRegistration<string, ParamRenameWhole>(
                new Option<string>(
                aliases: new string[] { "--change-extension-value" },
                description: "更改扩展名内容")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.ChangeExtensionValue = value);

            yield return new OptionRegistration<bool, ParamRenameWhole>(
                new Option<bool>(
                aliases: new string[] { "--is-auto-conflict-resolution" },
                description: "是否自动解决重命名冲突",
                getDefaultValue: () => true)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsAutomaticallyResolveRenameConflicts = value);
        }
    }
}
