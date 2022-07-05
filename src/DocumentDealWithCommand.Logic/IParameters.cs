using System.Collections.Generic;

namespace DocumentDealWithCommand.Logic
{
    /// <summary>
    /// 接口: 命令参数
    /// </summary>
    public interface IParameters
    {
        /// <summary>
        /// 写入日志参数
        /// </summary>
        /// <param name="logArgs">日志参数</param>
        void WriteLogArgs(IDictionary<string, object> logArgs);
    }
}
