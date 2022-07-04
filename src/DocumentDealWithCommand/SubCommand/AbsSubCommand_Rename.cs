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
    public abstract class AbsSubCommand_Rename<T> : AbsSubCommandImplementationVersion<T>, ISubCommand where T : AbsParamRename, new()
    {
        /// <inheritdoc/>
        public AbsSubCommand_Rename(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.RenameCommand ?? new string[] { }).ToArray();
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<T>> SetOptions()
        {
            yield return new OptionRegistration<bool, T>(
                new Option<bool>(
                aliases: new string[] { "--is-preview" },
                description: "是否预览",
                getDefaultValue: () => true)
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = false,
                },
                (param, value) => param.IsPreview = value,
                isGlobal: true);
        }
    }
}
