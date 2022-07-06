using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 获取命令内容
        /// </summary>
        /// <returns></returns>
        IContent GetContent();
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
