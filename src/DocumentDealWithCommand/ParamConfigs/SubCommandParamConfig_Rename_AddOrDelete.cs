using System.CommandLine;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename_AddOrDelete : SubCommandParamConfig_Rename<ParamRenameAddOrDelete>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename_AddOrDelete() : base()
        {
            new Option<string>(
                aliases: new string[] { "--before" },
                description: "文件名前添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.BeforeAdd = value);

            new Option<string>(
                aliases: new string[] { "--after" },
                description: "文件名后添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.AfterAdd = value);

            new Option<bool>(
                aliases: new string[] { "--is-use-extend-add" },
                description: "是否使用扩展添加",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.IsUseExtendAdd = value);

            new Option<uint>(
                aliases: new string[] { "--use-extend-add-start-index" },
                description: "从文件名第 `N` 个字符之后开始添加",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.UseExtendAddStartCharIndex = value);

            new Option<string>(
                aliases: new string[] { "--use-extend-add-content" },
                description: "扩展添加内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.UseExtendAddContent = value);

            new Option<string>(
                aliases: new string[] { "--delete" },
                description: "删除内容部分, 支持正则表达式")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.DeleteContent = value);

            new Option<bool>(
                aliases: new string[] { "--is-use-extend-delete" },
                description: "是否使用扩展删除",
                getDefaultValue: () => false)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.IsUseExtendDelete = value);

            new Option<uint>(
                aliases: new string[] { "--use-extend-delete-start-index" },
                description: "从文件名第 `N` 个字符开始删除",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.UseExtendDeleteStartCharIndex = value);

            new Option<uint>(
                aliases: new string[] { "--use-extend-delete-count" },
                description: "启用扩展删除, 需要删除的字符数量",
                getDefaultValue: () => 1)
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            }
            .Set(this, (param, value) => param.UseExtendDeleteCount = value);
        }
    }
}
