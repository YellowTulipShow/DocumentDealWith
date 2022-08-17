using System.Collections.Generic;
using System.CommandLine;

using YTS.Log;
using YTS.CodingSupportLibrary;

using CommandParamUse;
using CommandParamUse.Implementation;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <inheritdoc/>
    public class SubCommandParamConfig_Content_Encoding : AddParamConfigDefalutValue<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public SubCommandParamConfig_Content_Encoding() : base()
        {
            new Option<ESupportEncoding>(
                aliases: new string[] { "-t", "--target" },
                description: "目标编码配置")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            }.Set(this, (param, value) => param.Target = value);
        }
    }

    /// <summary>
    /// 子命令: 内容 - 编码修改
    /// </summary>
    public class SubCommand_Content_Encoding : ICommandSub<CommandParameters_Content_Encoding>
    {
        private readonly ILog log;

        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public string GetNameSign() => "encode";

        /// <inheritdoc/>
        public string GetDescription() => "重新配置编码";

        /// <inheritdoc/>
        public IExecute<CommandParameters_Content_Encoding> GetExecute()
        {
            return new Main_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public IParamConfig<CommandParameters_Content_Encoding> GetParamConfig()
        {
            return new SubCommandParamConfig_Content_Encoding();
        }

        /// <inheritdoc/>
        public IEnumerable<ICommandSub> GetSubCommands() => null;
    }
}
