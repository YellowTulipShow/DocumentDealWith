using System;
using System.IO;
using System.Linq;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 抽象类: 执行逻辑
    /// </summary>
    public abstract class AbsMain
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;

        /// <summary>
        /// 初始化实例执行逻辑
        /// </summary>
        /// <param name="log">日志接口</param>
        public AbsMain(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 获取可操作文件清单
        /// </summary>
        /// <param name="commandParameters">命令参数</param>
        /// <param name="allowExtensions">允许操作扩展名</param>
        /// <returns>文件清单</returns>
        protected FileInfo[] GetOperFileInventory(AbsBasicCommandParameters commandParameters, string[] allowExtensions)
        {
            string[] global_extensions = commandParameters
                .Config.Global_AllowExtensions ?? new string[] { };
            string[] extensions = global_extensions
                .Concat(allowExtensions ?? new string[] { }).ToArray();
            var calcFileInventory = new CalcCanOperationFileInventory(log, commandParameters.RootDire, extensions);
            calcFileInventory.Append(commandParameters.Files);
            calcFileInventory.Append(commandParameters.FileText);
            calcFileInventory.Append(commandParameters.Path, commandParameters.PathIsRecurse);
            FileInfo[] rinventory = calcFileInventory.GetResults();
            if (rinventory == null || rinventory.Length <= 0)
            {
                throw new ArgumentNullException("rinventory", "可操作文件清单为空列表, 请检查传入参数!");
            }
            return rinventory;
        }
    }
}
