using System;
using System.Linq;
using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;
using CommandParamUse;
using CommandParamUse.Implementation;
using YTS.CodingSupportLibrary;

namespace DocumentDealWithCommand.SubCommand
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename<P> : MainCommandParamConfig<P> where P : AbsParamRename, new()
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename() : base()
        {
            new Option<bool>(
                aliases: new string[] { "--is-preview" },
                description: "是否预览",
                getDefaultValue: () => true)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }.SetGlobal(this, (param, value) => param.IsPreview = value);

            new Option<uint>(
                aliases: new string[] { "--preview-column" },
                description: "预览展示列数量",
                getDefaultValue: () => 0)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }.SetGlobal(this, (param, value) => param.PreviewColumnCount = value);
        }
    }
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename_Whole : SubCommandParamConfig_Rename<ParamRenameWhole>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename_Whole() : base()
        {
            new Option<string>(
                aliases: new string[] { "--rule" },
                description: "命名规则")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }
            .Set(this, (param, value) => param.NamingRules = value);

            new Option<uint>(
                aliases: new string[] { "--start" },
                description: "开始于",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.StartedOnIndex = value);

            new Option<uint>(
                aliases: new string[] { "--increment" },
                description: "增量",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.Increment = value);

            new Option<uint>(
                aliases: new string[] { "--digit" },
                description: "位数",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.Digit = value);

            new Option<bool>(
                aliases: new string[] { "--is-repair-bit" },
                description: "是否不足位补齐",
                getDefaultValue: () => true)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.IsInsufficientSupplementBit = value);

            new Option<bool>(
                aliases: new string[] { "--is-use-letter" },
                description: "是否使用字母",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.IsUseLetter = value);

            new Option<ERenameLetterFormat>(
                aliases: new string[] { "--use-letter-format" },
                description: "使用字母类型格式",
                getDefaultValue: () => ERenameLetterFormat.Lower)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.UseLetterFormat = value);

            new Option<bool>(
                aliases: new string[] { "--is-change-extension" },
                description: "是否更改扩展名",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.IsChangeExtension = value);

            new Option<string>(
                aliases: new string[] { "--change-extension-value" },
                description: "更改扩展名内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.ChangeExtensionValue = value);
        }
    }

    /// <summary>
    /// 子命令: 重命名
    /// </summary>
    public class SubCommand_Rename_Whole : ICommandSub<ParamRenameWhole>
    {
        private readonly ILog log;

        /// <inheritdoc/>
        public SubCommand_Rename_Whole(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public string GetNameSign() => "rename";

        /// <inheritdoc/>
        public string GetDescription() => "内容操作";

        /// <inheritdoc/>
        public IExecute<ParamRenameWhole> GetExecute()
        {
            return new Main_Rename_Whole(log);
        }

        /// <inheritdoc/>
        public IParamConfig<ParamRenameWhole> GetParamConfig()
        {
            return new SubCommandParamConfig_Rename_Whole();
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new SubCommand_Rename_Replace(log);
            yield return new SubCommand_Rename_AddOrDelete(log);
        }
    }
}
