using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 换行符类型更改
    /// </summary>
    public class Main_Content_NewLine : AbsMain, IMain<CommandParameters_Content_NewLine>
    {
        /// <inheritdoc/>
        public Main_Content_NewLine(ILog log, IPrintColor print) : base(log, print) { }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Content_NewLine commandParameters)
        {
            print.WriteLine("未实现更改换行符逻辑~~~", EPrintColor.Red);
            return 2;
        }
    }
}
