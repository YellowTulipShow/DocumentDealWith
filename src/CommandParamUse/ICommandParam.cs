using System.CommandLine;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令
    /// </summary>
    public interface ICommandParam<T, P> : ICommand<T> where T : Command where P : IParam
    {
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
}
