using System;
using System.CommandLine;

using YTS.Log;

using CodingSupportLibrary;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容 - 编码修改
    /// </summary>
    public class SubCommand_Content_Encoding : SubCommand_Content, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content_Encoding(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Option<ESupportEncoding> option_TargetEncoding = GetOption_TargetEncoding();
            Command cmd = new Command("encode", "重新配置编码");
            cmd.AddOption(option_TargetEncoding);
            cmd.SetHandler((context) =>
            {
                var logArgs = log.CreateArgDictionary();
                try
                {
                    var commandOptions = ToCommandParameters<CommandParameters_Content_Encoding>(context);
                    commandOptions.Target = context.ParseResult.GetValueForOption(option_TargetEncoding);
                    var main = new Main_Content_Encoding(log, commandOptions.Print);
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

        private Option<ESupportEncoding> GetOption_TargetEncoding()
        {
            var option = new Option<ESupportEncoding>(
                aliases: new string[] { "-t", "--target" },
                description: "目标编码配置")
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
