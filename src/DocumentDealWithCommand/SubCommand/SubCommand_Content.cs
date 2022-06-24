using System.Linq;
using System.CommandLine;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容相关操作
    /// </summary>
    public class SubCommand_Content : AbsSubCommand, ISubCommand
    {
        /// <inheritdoc/>
        public SubCommand_Content(ILog log, GlobalOptions globalOptions) : base(log, globalOptions)
        {
        }

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            Command cmd = new Command("content", "内容操作");
            ISubCommand[] subCommands = new ISubCommand[]
            {
                new SubCommand_Content_Encoding(log, globalOptions),
                new SubCommand_Content_NewLine(log, globalOptions),
            };
            foreach (var sub in subCommands)
            {
                Command subCMD = sub.GetCommand();
                cmd.AddCommand(subCMD);
            }
            return cmd;
        }

        /// <inheritdoc/>
        protected override string[] GetConfigAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.ContentCommand ?? new string[] { }).ToArray();
        }
    }
}
