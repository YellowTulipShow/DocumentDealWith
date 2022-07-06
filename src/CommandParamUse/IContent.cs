using System.Collections.Generic;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令内容
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// 获取子命令项
        /// </summary>
        IEnumerable<ISubCommand> GetSubCommands();

        /// <summary>
        /// 获取执行方法
        /// </summary>
        /// <returns>执行对象</returns>
        IExecute GetExecute();
    }
}
