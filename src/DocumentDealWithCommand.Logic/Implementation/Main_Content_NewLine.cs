using System;
using System.Collections.Generic;
using System.Text;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 换行符类型更改
    /// </summary>
    public class Main_Content_NewLine : IMain<CommandParameters_Content_NewLine>
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化实现命令类
        /// </summary>
        /// <param name="log">日志接口</param>
        public Main_Content_NewLine(ILog log)
        {
            this.log = log;
        }

        /// <inheritdoc/>
        public void OnExecute(CommandParameters_Content_NewLine commandParameters)
        {
            log.Info($"转换换行符, 参数 - Type: {commandParameters.Type}");
        }
    }
}
