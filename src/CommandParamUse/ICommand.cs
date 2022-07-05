using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令
    /// </summary>
    /// <typeparam name="TCmd">命令类型</typeparam>
    public interface ICommand<TCmd>
        where TCmd : Command
    {
        /// <summary>
        /// 获取命令
        /// </summary>
        /// <returns></returns>
        TCmd GetCommand();

        /// <summary>
        /// 获取命令内容
        /// </summary>
        /// <returns></returns>
        ICommandContent GetContent();
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
