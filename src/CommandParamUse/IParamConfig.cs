using System.Collections.Generic;

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
    }
}
