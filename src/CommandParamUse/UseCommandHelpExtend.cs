using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 静态扩展类: 调用命令解析程序实现帮助类
    /// </summary>
    public static class UseCommandHelpExtend
    {
        public static RootCommand ToCommand<P>(this IRootCommand<P> root) where P : IParam
        {
            return new RootCommand(root.GetDescription());
        }
        public static Command ToCommand<P>(this ISubCommand<P> root) where P : IParam
        {
            return new Command(root.GetNameSign(), root.GetDescription());
        }
    }
}
