using System;
using System.CommandLine;
using System.Linq;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 重命名
    /// </summary>
    public class SubCommand_Rename : AbsSubCommand, ISubCommand
    {
        private readonly IMain<CommandParameters_Rename> main;

        /// <inheritdoc/>
        public SubCommand_Rename(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
            main = new Main_Rename(log);
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Option<string> option_outname = GetOption_OutName();
            Command cmd = new Command("rename", "重命名文件");
            cmd.AddOption(option_outname);
            cmd.SetHandler((context) =>
            {
                var logArgs = log.CreateArgDictionary();
                try
                {
                    var commandOptions = globalOptions
                        .ToCommandParameters<CommandParameters_Rename>(log, context);
                    commandOptions.OutputName = context.ParseResult.GetValueForOption(option_outname);
                    context.ExitCode = main.OnExecute(commandOptions);
                }
                catch (Exception ex)
                {
                    log.Error("重命名文件 - 执行出错", ex, logArgs);
                    context.ExitCode = 1;
                }
            });
            return cmd;
        }
        private Option<string> GetOption_OutName()
        {
            var option = new Option<string>(
                aliases: new string[] { "-o", "--outname" },
                description: "输出名称配置",
                parseArgument: result =>
                {
                    if (result.Tokens.Count == 0)
                    {
                        result.ErrorMessage = "输出名称配置是必填项!";
                        return null;
                    }
                    string outname = result.Tokens.Single().Value;
                    if (string.IsNullOrEmpty(outname))
                    {
                        result.ErrorMessage = "输出名称配置不能为空字符串!";
                        return null;
                    }
                    return outname;
                })
            {
                Arity = ArgumentArity.ExactlyOne,
                IsRequired = true,
            };
            return option;
        }
    }
}
