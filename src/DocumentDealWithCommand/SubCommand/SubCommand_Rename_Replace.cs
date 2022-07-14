using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名 - 替换
    /// </summary>
    public class SubCommand_Rename_Replace : AbsSubCommand_Rename<ParamRenameReplace>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename_Replace(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "replace";

        /// <inheritdoc/>
        public override string CommandDescription() => "替换";

        /// <inheritdoc/>
        public override IMain<ParamRenameReplace> HandlerLogic()
        {
            return new Main_Rename_Replace(log);
        }

        /// <inheritdoc/>
        public override IEnumerable<IOptionRegistration<ParamRenameReplace>> SetOptions()
        {
            var base_list = base.SetOptions();
            if (base_list != null)
            {
                foreach (var base_item in base_list)
                {
                    yield return base_item;
                }
            }

            yield return new OptionRegistration<string, ParamRenameReplace>(
                new Option<string>(
                aliases: new string[] { "--pattern" },
                description: "匹配项, 支持正则表达式")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = true,
                },
                (param, value) => param.Pattern = value);

            yield return new OptionRegistration<string, ParamRenameReplace>(
                new Option<string>(
                aliases: new string[] { "--replacement" },
                description: "替换内容")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    IsRequired = true,
                },
                (param, value) => param.Replacement = value);
        }
    }
}
