using System.Collections.Generic;

namespace CommandParamUse.Implementation
{

    /// <summary>
    /// 实现类: 添加参数配置实现类
    /// </summary>
    /// <typeparam name="P">参数对象</typeparam>
    public class AddParamConfig<P> : IParamConfig<P> where P : IParam
    {
        /// <summary>
        /// 参数对象
        /// </summary>
        protected readonly P param;

        /// <summary>
        /// 全局输入项
        /// </summary>
        protected readonly IList<IInputOption<P>> inputs_global;

        /// <summary>
        /// 正常输入项
        /// </summary>
        protected readonly IList<IInputOption<P>> inputs;

        /// <summary>
        /// 实例化: 添加参数配置实现类
        /// </summary>
        /// <param name="param">参数对象</param>
        public AddParamConfig(P param)
        {
            this.param = param;
            inputs_global = new List<IInputOption<P>>();
            inputs = new List<IInputOption<P>>();
        }

        /// <inheritdoc/>
        public P CreateParam() => param;

        /// <inheritdoc/>
        public IEnumerable<IInputOption<P>> GetGlobalInputs()
        {
            return inputs_global;
        }

        /// <inheritdoc/>
        public IEnumerable<IInputOption<P>> GetInputs()
        {
            return inputs;
        }

        /// <summary>
        /// 添加一项输入配置项
        /// </summary>
        /// <param name="inputOption">输入配置项</param>
        /// <param name="isGlobal">是否标识为全局</param>
        public void AddInputOption(IInputOption<P> inputOption, bool isGlobal = false)
        {
            if (isGlobal)
            {
                inputs_global.Add(inputOption);
            }
            else
            {
                inputs.Add(inputOption);
            }
        }

        /// <inheritdoc/>
        public virtual P ParameterProcess(P param) => param;
    }
}
