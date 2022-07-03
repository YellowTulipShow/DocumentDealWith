using System;
using System.Linq;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;
using System.Collections.Generic;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 添加或删除
    /// </summary>
    public class SubCommand_Rename_AddOrDelete : AbsSubCommandImplementationVersion<CommandParameters_Rename_AddOrDelete>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename_AddOrDelete(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "rename";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Rename_AddOrDelete> HandlerLogic()
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
        public override IEnumerable<IOptionRegistration<CommandParameters_Rename_AddOrDelete>> SetOptions()
        {
            yield return new OptionRegistration<bool, CommandParameters_Rename_AddOrDelete>
                (GetOption_IsPreview(), (param, value) => param.IsPreview = value);

            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_BeforeAdd(), (param, value) => param.BeforeAdd = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_AfterAdd(), (param, value) => param.AfterAdd = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_IsUseExtendAdd(), (param, value) => param.IsUseExtendAdd = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename_AddOrDelete>
                (GetOption_UseExtendAddStartCharIndex(), (param, value) => param.UseExtendAddStartCharIndex = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_UseExtendAddContent(), (param, value) => param.UseExtendAddContent = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_DeleteContent(), (param, value) => param.DeleteContent = value);
            yield return new OptionRegistration<string, CommandParameters_Rename_AddOrDelete>
                (GetOption_IsUseExtendDelete(), (param, value) => param.IsUseExtendDelete = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename_AddOrDelete>
                (GetOption_UseExtendDeleteStartCharIndex(), (param, value) => param.UseExtendDeleteStartCharIndex = value);
            yield return new OptionRegistration<uint, CommandParameters_Rename_AddOrDelete>
                (GetOption_UseExtendDeleteCount(), (param, value) => param.UseExtendDeleteCount = value);
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
        private Option<string> GetOption_BeforeAdd()
        {
            var option = new Option<string>(
                aliases: new string[] { "--BeforeAdd" },
                description: "文件名前添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_AfterAdd()
        {
            var option = new Option<string>(
                aliases: new string[] { "--AfterAdd" },
                description: "文件名后添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_IsUseExtendAdd()
        {
            var option = new Option<string>(
                aliases: new string[] { "--IsUseExtendAdd" },
                description: "是否使用扩展添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_UseExtendAddStartCharIndex()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--UseExtendAddStartCharIndex" },
                description: "从文件名第 `N` 个字符之后开始添加")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_UseExtendAddContent()
        {
            var option = new Option<string>(
                aliases: new string[] { "--UseExtendAddContent" },
                description: "扩展添加内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_DeleteContent()
        {
            var option = new Option<string>(
                aliases: new string[] { "--DeleteContent" },
                description: "删除内容部分, 支持正则表达式")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<string> GetOption_IsUseExtendDelete()
        {
            var option = new Option<string>(
                aliases: new string[] { "--IsUseExtendDelete" },
                description: "是否使用扩展删除")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_UseExtendDeleteStartCharIndex()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--UseExtendDeleteStartCharIndex" },
                description: "从文件名第 `N` 个字符开始删除")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
        private Option<uint> GetOption_UseExtendDeleteCount()
        {
            var option = new Option<uint>(
                aliases: new string[] { "--UseExtendDeleteCount" },
                description: "启用扩展删除, 需要删除的字符数量")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
