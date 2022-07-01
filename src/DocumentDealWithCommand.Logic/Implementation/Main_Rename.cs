using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名
    /// </summary>
    public class Main_Rename : AbsMain, IMain<CommandParameters_Rename>
    {
        /// <inheritdoc/>
        public Main_Rename(ILog log, IPrintColor print) : base(log, print) { }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Rename commandParameters)
        {
            print.WriteLine("未实现重命名逻辑~~~", EPrintColor.Red);
            return 2;
        }
    }
}
