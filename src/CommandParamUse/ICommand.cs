using System.Collections.Generic;
using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 获取执行方法
        /// </summary>
        /// <returns>执行对象</returns>
        IExecute GetExecute();

        /// <summary>
        /// 获取子命令项
        /// </summary>
        IEnumerable<ISubCommand> GetSubCommands();
    }

    /// <summary>
    /// 接口: 命令
    /// </summary>
    /// <typeparam name="TCmd">命令类型</typeparam>
    public interface ICommand<TCmd> : ICommand
        where TCmd : Command
    {
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        TCmd GetCommand();
    }

    /// <summary>
    /// 根命令
    /// </summary>
    public interface IRootCommand : ICommand<RootCommand> { }

    /// <summary>
    /// 子命令
    /// </summary>
    public interface ISubCommand : ICommand<Command> { }
}
