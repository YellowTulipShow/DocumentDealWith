using System.Linq;
using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 内容相关操作
    /// </summary>
    public class SubCommand_Content : AbsUseLog, ICommandSub
    {
        /// <inheritdoc/>
        public SubCommand_Content(ILog log) : base(log) { }

        /// <inheritdoc/>
        public string GetNameSign() => "content";

        /// <inheritdoc/>
        public string GetDescription() => "内容操作";

        /// <inheritdoc/>
        public virtual IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new SubCommand_Content_Encoding(log);
            yield return new SubCommand_Content_NewLine(log);
        }

        /// <summary>
        /// 取公共配置+内容配置
        /// </summary>
        public static string[] CalcAllowExtensions(Configs config)
        {
            return (config.AllowExtension.Global ?? new string[] { }).Concat(
                config.AllowExtension.ContentCommand ?? new string[] { }).ToArray();
        }
    }
}
