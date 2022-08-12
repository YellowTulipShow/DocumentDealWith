using System.Collections.Generic;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令
    /// </summary>
    public interface ICommand<P> where P: IParam
    {
        /// <summary>
        /// 获取命令描述
        /// </summary>
        /// <returns>字符串描述</returns>
        string GetDescription();

        /// <summary>
        /// 获取子命令
        /// </summary>
        /// <typeparam name="SubP">子命令参数类型</typeparam>
        /// <returns>子命令列表</returns>
        IList<ISubCommand<SubP>> GetSubCommands<SubP>() where SubP : P;

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
    public interface IRootCommand<P> : ICommand<P> where P : IParam { }

    /// <summary>
    /// 子命令
    /// </summary>
    public interface ISubCommand<P> : ICommand<P> where P : IParam
    {
        /// <summary>
        /// 获取命令标识
        /// </summary>
        /// <returns>字符串标识</returns>
        string GetNameSign();
    }
}
