using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 替换
    /// </summary>
    public class Main_Rename_Replace : AbsMain, IMain<ParamRenameReplace>
    {
        /// <inheritdoc/>
        public Main_Rename_Replace(ILog log) : base(log) { }

        /// <inheritdoc/>
        public int OnExecute(ParamRenameReplace commandParameters)
        {
            commandParameters.Print.WriteLine("未实现重命名-替换逻辑~~~", EPrintColor.Red);
            return 2;
        }
    }
}
