using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;
using YTS.ConsolePrint;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 编码修改
    /// </summary>
    public class Main_Content_Encoding : AbsMain, IMain<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public Main_Content_Encoding(ILog log, IPrintColor print) : base(log, print) { }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Content_Encoding commandParameters)
        {
            log.Info($"转换编码, 参数 - Encoding: {commandParameters.Encoding}");

            var rinventory = commandParameters.NeedHandleFileInventory;
            if (rinventory == null || rinventory.Length <= 0)
            {
                Console.WriteLine($"可操作文件清单为空!");
                return 1;
            }
            foreach (var item in rinventory)
            {
                Console.WriteLine($"文件路径: {item.FullName}");
            }
            return 0;
        }
    }
}
