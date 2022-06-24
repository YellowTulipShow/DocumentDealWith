using System;
using System.CommandLine;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 换行符
    /// </summary>
    public class SubCommand_Content_NewLine : SubCommand_Content, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content_NewLine(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Option<ENewLineType> option_Type = GetOption_Type();
            Command cmd = new Command("newline", "重新配置换行符");
            cmd.AddOption(option_Type);
            cmd.SetHandler((context) =>
            {
                var logArgs = log.CreateArgDictionary();
                try
                {
                    var commandOptions = ToCommandParameters<CommandParameters_Content_NewLine>(context);
                    commandOptions.Type = context.ParseResult.GetValueForOption(option_Type);
                    var main = new Main_Content_NewLine(log, commandOptions.Print);
                    context.ExitCode = main.OnExecute(commandOptions);
                }
                catch (Exception ex)
                {
                    log.Error("重新配置编码 - 执行出错", ex, logArgs);
                    context.ExitCode = 1;
                }
            });
            return cmd;
        }

        private Option<ENewLineType> GetOption_Type()
        {
            var option = new Option<ENewLineType>(
                aliases: new string[] { "-t", "--type" },
                description: "目标换行标识")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
