using System;
using System.Linq;
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
    public class SubCommand_Rename : AbsSubCommandImplementationVersion<CommandParameters_Rename>, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Rename(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <inheritdoc/>
        public override string CommandNameSign() => "rename";

        /// <inheritdoc/>
        public override string CommandDescription() => "内容操作";

        /// <inheritdoc/>
        public override IMain<CommandParameters_Rename> HandlerLogic()
        {
            return new Main_Rename(log);
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.RenameCommand ?? new string[] { }).ToArray();
        }
        /// <summary>
        /// 取公共配置+重命名配置
        /// </summary>
        public static string[] CalcAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.RenameCommand ?? new string[] { }).ToArray();
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
