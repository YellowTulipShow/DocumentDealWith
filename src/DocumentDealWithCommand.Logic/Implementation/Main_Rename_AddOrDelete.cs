using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 添加/删除
    /// </summary>
    public class Main_Rename_AddOrDelete : AbsMain, IMain<ParamRenameAddOrDelete>
    {
        /// <inheritdoc/>
        public Main_Rename_AddOrDelete(ILog log) : base(log) { }

        /// <inheritdoc/>
        public int OnExecute(ParamRenameAddOrDelete commandParameters)
        {
            commandParameters.Print.WriteLine("未实现重命名 - 添加/删除逻辑~~~", EPrintColor.Red);
            return 2;
        }
    }
}
