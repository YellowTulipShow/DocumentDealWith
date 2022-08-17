using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Rename_Replace : SubCommandParamConfig_Rename<ParamRenameReplace>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Rename_Replace() : base()
        {
            new Option<string>(
                aliases: new string[] { "--pattern" },
                description: "匹配项, 支持正则表达式")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }
            .Set(this, (param, value) => param.Pattern = value);

            new Option<string>(
                aliases: new string[] { "--replacement" },
                description: "替换内容")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }
            .Set(this, (param, value) => param.Replacement = value);
        }
    }
    /// <summary>
    /// 子命令: 重命名 - 替换
    /// </summary>
    public class SubCommand_Rename_Replace : ICommandSub<ParamRenameReplace>
    {
        private readonly ILog log;

        /// <inheritdoc/>
        public SubCommand_Rename_Replace(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public string GetNameSign() => "replace";

        /// <inheritdoc/>
        public string GetDescription() => "替换";

        /// <inheritdoc/>
        public IExecute<ParamRenameReplace> GetExecute()
        {
            return new Main_Rename_Replace(log);
        }

        /// <inheritdoc/>
        public IParamConfig<ParamRenameReplace> GetParamConfig()
        {
            return new SubCommandParamConfig_Rename_Replace();
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
