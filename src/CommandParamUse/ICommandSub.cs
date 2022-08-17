using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 子命令
    /// </summary>
    public interface ICommandSub : ICommand<Command>
    {
        /// <summary>
        /// 获取命令标识
        /// </summary>
        /// <returns>字符串标识</returns>
        string GetNameSign();

        Command ICommand<Command>.GetCommand()
        {
            Command cmd = new Command(GetNameSign(), GetDescription());
            cmd = cmd.FillCommandContent(this);
            return cmd;
        }
    }

    /// <summary>
    /// 接口: 指定明确参数的子命令
    /// </summary>
    public interface ICommandSub<P> : ICommandSub, ICommandParam<Command, P> where P : IParam
    {
        Command ICommand<Command>.GetCommand()
        {
            Command cmd = new Command(GetNameSign(), GetDescription());
            cmd = cmd.FillParamCommandContent(this);
            return cmd;
        }
    }
}
