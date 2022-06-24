using DocumentDealWithCommand.Logic.Models;

using YTS.ConsolePrint;
using YTS.Log;

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
            log.Info($"执行重命名逻辑内容, 参数 - OutputName: {commandParameters.OutputName}");
            return 0;
        }
    }
}
