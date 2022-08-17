namespace CommandParamUse.Implementation
{
    /// <summary>
    /// 接口: 添加参数配置实现类
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public interface IAddParamConfig<P> : IParamConfig<P> where P : IParam
    {
        /// <summary>
        /// 添加一项输入配置
        /// </summary>
        /// <param name="inputOption">输入配置项</param>
        /// <param name="isGlobal">是否是全局</param>
        void AddInputOption(IInputOption<P> inputOption, bool isGlobal = false);
    }
}
