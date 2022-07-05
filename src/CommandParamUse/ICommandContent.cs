using System.Collections.Generic;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令内容
    /// </summary>
    public interface ICommandContent
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

    /// <summary>
    /// 接口: 执行方案
    /// </summary>
    public interface IExecute
    {
        /// <summary>
        /// 获取全局配置参数项
        /// </summary>
        IEnumerable<IOption> GetGlobalOptions();

        /// <summary>
        /// 获取配置参数项
        /// </summary>
        IEnumerable<IOption> GetOptions();

        /// <summary>
        /// 是否需要执行逻辑
        /// </summary>
        bool IsExecute();

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">命令参数解析上下文</param>
        /// <returns>返回数字标识</returns>
        int OnExecute(InvocationContext context);
    }

    public interface IExecute<TParam> : IExecute where TParam : IParameters
    {
        /// <summary>
        /// 获取配置参数项
        /// </summary>
        new IEnumerable<IOption<TParam>> GetOptions();
        /// <summary>
        /// 获取基础参数
        /// </summary>
        /// <returns>参数对象</returns>
        TParam GetParam();

        /// <summary>
        /// 根据参数对象执行逻辑
        /// </summary>
        /// <param name="context">命令参数解析上下文</param>
        /// <param name="param">参数</param>
        /// <returns>返回数字标识</returns>
        int OnExecute(InvocationContext context, TParam param);
    }
}
