using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 根命令
    /// </summary>
    public interface ICommandRoot : ICommand<RootCommand>
    {
        RootCommand ICommand<RootCommand>.GetCommand()
        {
            RootCommand cmd = new RootCommand(GetDescription());
            cmd = cmd.FillCommandContent(this);
            return cmd;
        }
    }

    /// <summary>
    /// 接口: 指定明确参数的根命令
    /// </summary>
    public interface ICommandRoot<P> : ICommandRoot, ICommandParam<RootCommand, P> where P : IParam
    {
        RootCommand ICommand<RootCommand>.GetCommand()
        {
            RootCommand cmd = new RootCommand(GetDescription());
            cmd = cmd.FillParamCommandContent(this);
            return cmd;
        }
    }
}
