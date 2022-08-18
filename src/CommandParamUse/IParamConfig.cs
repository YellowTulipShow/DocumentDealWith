using System.Collections.Generic;

using YTS.Log;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 参数配置
    /// </summary>
    public interface IParamConfig<P> where P : IParam
    {
        /// <summary>
        /// 生成创建参数对象
        /// </summary>
        public P CreateParam();

        /// <summary>
        /// 获取全局配置输入项
        /// </summary>
        public IEnumerable<IInputOption<P>> GetGlobalInputs();

        /// <summary>
        /// 获取配置输入项
        /// </summary>
        public IEnumerable<IInputOption<P>> GetInputs();

        /// <summary>
        /// 参数处理方法
        /// </summary>
        /// <param name="param">参数处理</param>
        /// <param name="log">日志接口</param>
        /// <returns>处理后的参数</returns>
        public P ParameterProcess(P param, ILog log);
    }
}
