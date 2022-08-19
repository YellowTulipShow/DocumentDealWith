using YTS.Log;

namespace DocumentDealWithCommand.Logic
{
    /// <summary>
    /// 抽象类: 基础使用日志类
    /// </summary>
    public abstract class AbsUseLog
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;

        /// <summary>
        /// 初始化: 基础使用日志类
        /// </summary>
        /// <param name="log">日志接口</param>
        public AbsUseLog(ILog log)
        {
            this.log = log;
        }
    }
}
