using System;
using System.Collections.Generic;
using System.Text;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名
    /// </summary>
    public class Main_Rename : IMain<CommandParameters_Rename>
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化实现命令类
        /// </summary>
        /// <param name="log">日志接口</param>
        public Main_Rename(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Rename commandParameters)
        {
            log.Info($"执行重命名逻辑内容, 参数 - OutputName: {commandParameters.OutputName}");
            return 0;
        }
    }
}
