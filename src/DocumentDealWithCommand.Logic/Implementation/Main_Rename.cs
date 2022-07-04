using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名
    /// </summary>
    public class Main_Rename : AbsMain, IMain<ParamRenameWhole>
    {
        /// <inheritdoc/>
        public Main_Rename(ILog log) : base(log) { }

        /// <inheritdoc/>
        public int OnExecute(ParamRenameWhole commandParameters)
        {
            commandParameters.Print.WriteLine("未实现重命名逻辑~~~", EPrintColor.Red);
            return 2;
        }
    }
}
