using System.CommandLine;

using YTS.Log;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 抽象基类实现
    /// </summary>
    public abstract class AbsSubCommand : ISubCommand
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// 全局配置项
        /// </summary>
        protected readonly GlobalOptions globalOptions;

        /// <summary>
        /// 实例化子命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="globalOptions">全局配置项</param>
        public AbsSubCommand(ILog log, GlobalOptions globalOptions)
        {
            this.log = log;
            this.globalOptions = globalOptions;
        }

        /// <inheritdoc/>
        public abstract Command GetCommand();
    }
}
