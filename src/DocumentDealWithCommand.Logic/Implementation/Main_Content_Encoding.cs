using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 编码修改
    /// </summary>
    public class Main_Content_Encoding : IMain<CommandParameters_Content_Encoding>
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化实现命令类
        /// </summary>
        /// <param name="log">日志接口</param>
        public Main_Content_Encoding(ILog log)
        {
            this.log = log;
            
        }

        /// <inheritdoc/>
        public void OnExecute(CommandParameters_Content_Encoding commandParameters)
        {
            log.Info($"转换编码, 参数 - Encoding: {commandParameters.Encoding}");

            //DirectoryInfo rootDire = new DirectoryInfo(Environment.CurrentDirectory);
            //string[] allowExtensions = commandParameters.Config.Global_AllowExtensions
            //    .Concat(commandParameters.Config.ContentCommand_AllowExtensions)
            //    .ToArray();
            //var calcFileInventory = new CalcCanOperationFileInventory(log, rootDire, allowExtensions);
            //calcFileInventory.Append(commandParameters.Files);
            //calcFileInventory.Append(commandParameters.FileText);
            //calcFileInventory.Append(commandParameters.Path, commandParameters.PathIsRecurse);
            //FileInfo[] rinventory = calcFileInventory.GetResults();
            //foreach (var item in rinventory)
            //{
            //    Console.WriteLine($"文件路径: {item.FullName}");
            //}
        }
    }
}
