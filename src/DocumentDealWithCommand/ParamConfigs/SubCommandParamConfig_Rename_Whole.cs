using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
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
}
