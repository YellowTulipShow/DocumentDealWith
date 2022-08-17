using System.Collections.Generic;
using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 类型命令
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public interface ICommand<T> where T : Command
    {
        /// <summary>
        /// 获取命令描述
        /// </summary>
        /// <returns>字符串描述</returns>
        string GetDescription();

        /// <summary>
        /// 获取命令
        /// </summary>
        public T GetCommand();

        /// <summary>
        /// 获取子命令
        /// </summary>
        /// <returns>子命令列表</returns>
        IEnumerable<ICommandSub> GetSubCommands();
    }
}
