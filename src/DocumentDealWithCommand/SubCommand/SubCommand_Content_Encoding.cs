using System;
using System.Text;
using System.CommandLine;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 编码修改
    /// </summary>
    public class SubCommand_Content_Encoding : AbsSubCommand, ISubCommand
    {
        private readonly IMain<CommandParameters_Content_Encoding> main;

        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
            main = new Main_Content_Encoding(log);
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Option<Encoding> option_Encoding = GetOption_Encoding();
            Command cmd = new Command("encode", "重新配置编码");
            cmd.AddOption(option_Encoding);
            cmd.SetHandler((context) =>
            {
                var logArgs = log.CreateArgDictionary();
                try
                {
                    CommandParameters_Content_Encoding commandOptions = globalOptions.ToCommandParameters<CommandParameters_Content_Encoding>(context);
                    commandOptions.Encoding = context.ParseResult.GetValueForOption(option_Encoding);
                    main.OnExecute(commandOptions);
                }
                catch (Exception ex)
                {
                    log.Error("重新配置编码 - 执行出错", ex, logArgs);
                    context.ExitCode = 1;
                }
            });
            return cmd;
        }
        private Option<Encoding> GetOption_Encoding()
        {
            var option = new Option<Encoding>(
                aliases: new string[] { "-t", "--Target" },
                getDefaultValue => Encoding.UTF8,
                description: "目标编码配置")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
