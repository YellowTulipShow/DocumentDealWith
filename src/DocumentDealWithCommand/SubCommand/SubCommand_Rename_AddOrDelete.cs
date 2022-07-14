using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 添加或删除
    /// </summary>
    public class SubCommand_Rename_AddOrDelete : AbsSubCommand_Rename<ParamRenameAddOrDelete>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename_AddOrDelete(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "ad";

        /// <inheritdoc/>
        public override string CommandDescription() => "添加或删除";

        /// <inheritdoc/>
        public override IMain<ParamRenameAddOrDelete> HandlerLogic()
        {
            return new Main_Rename_AddOrDelete(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<ParamRenameAddOrDelete>> SetOptions()
        {
            var base_list = base.SetOptions();
            if (base_list != null)
            {
                foreach (var base_item in base_list)
                {
                    yield return base_item;
                }
            }

            yield return new OptionRegistration<string, ParamRenameAddOrDelete>(new Option<string>(
                aliases: new string[] { "--before" },
                description: "文件名前添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = false,
            },
                (param, value) => param.BeforeAdd = value);

            yield return new OptionRegistration<string, ParamRenameAddOrDelete>(
                new Option<string>(
                aliases: new string[] { "--after" },
                description: "文件名后添加")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.AfterAdd = value);

            yield return new OptionRegistration<bool, ParamRenameAddOrDelete>(
                new Option<bool>(
                aliases: new string[] { "--is-use-extend-add" },
                description: "是否使用扩展添加",
                getDefaultValue: () => false)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsUseExtendAdd = value);

            yield return new OptionRegistration<uint, ParamRenameAddOrDelete>(
                new Option<uint>(
                aliases: new string[] { "--use-extend-add-start-index" },
                description: "从文件名第 `N` 个字符之后开始添加",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.UseExtendAddStartCharIndex = value);

            yield return new OptionRegistration<string, ParamRenameAddOrDelete>(
                new Option<string>(
                aliases: new string[] { "--use-extend-add-content" },
                description: "扩展添加内容")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.UseExtendAddContent = value);

            yield return new OptionRegistration<string, ParamRenameAddOrDelete>(
                new Option<string>(
                aliases: new string[] { "--delete" },
                description: "删除内容部分, 支持正则表达式")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.DeleteContent = value);

            yield return new OptionRegistration<bool, ParamRenameAddOrDelete>(
                new Option<bool>(
                aliases: new string[] { "--is-use-extend-delete" },
                description: "是否使用扩展删除",
                getDefaultValue: () => false)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsUseExtendDelete = value);

            yield return new OptionRegistration<uint, ParamRenameAddOrDelete>(
                new Option<uint>(
                aliases: new string[] { "--use-extend-delete-start-index" },
                description: "从文件名第 `N` 个字符开始删除",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.UseExtendDeleteStartCharIndex = value);

            yield return new OptionRegistration<uint, ParamRenameAddOrDelete>(
                new Option<uint>(
                aliases: new string[] { "--use-extend-delete-count" },
                description: "启用扩展删除, 需要删除的字符数量",
                getDefaultValue: () => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.UseExtendDeleteCount = value);
        }
    }
}
