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
        /// 获取命令
        /// </summary>
        public T GetCommand();

        /// <summary>
        /// 获取子命令
        /// </summary>
        /// <returns>子命令列表</returns>
        IEnumerable<ISubCommand> GetSubCommands();
    }

    /// <summary>
    /// 接口: 子级命令配置
    /// </summary>
    public interface ISubCommand : ICommand<Command>
    {
        /// <summary>
        /// 获取命令标识
        /// </summary>
        /// <returns>字符串标识</returns>
        string GetNameSign();
    }

    /// <summary>
    /// 接口: 命令
    /// </summary>
    public interface IParamCommand<T, P> : ICommand<T> where T : Command where P: IParam
    {
        /// <summary>
        /// 获取命令描述
        /// </summary>
        /// <returns>字符串描述</returns>
        string GetDescription();

        /// <summary>
        /// 获取执行方法
        /// </summary>
        /// <returns>执行对象</returns>
        IExecute<P> GetExecute();

        /// <summary>
        /// 获取参数配置
        /// </summary>
        /// <returns>配置项</returns>
        IParamConfig<P> GetParamConfig();
    }

    /// <summary>
    /// 根命令
    /// </summary>
    public interface IRootCommand<P> : IParamCommand<RootCommand, P> where P : IParam
    {
        RootCommand ICommand<RootCommand>.GetCommand()
        {
            return this.ToCommand();
        }
    }

    /// <summary>
    /// 子命令
    /// </summary>
    public interface ISubCommand<P> : ISubCommand, IParamCommand<Command, P> where P : IParam
    {
        Command ICommand<Command>.GetCommand()
        {
            return this.ToCommand();
        }
    }
}
