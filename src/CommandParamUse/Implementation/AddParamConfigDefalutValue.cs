namespace CommandParamUse.Implementation
{
    /// <summary>
    /// 添加参数配置实现类
    /// </summary>
    /// <typeparam name="P">参数对象</typeparam>
    public class AddParamConfigDefalutValue<P> : AddParamConfig<P> where P : IParam, new()
    {
        /// <summary>
        /// 实例化: 添加参数配置实现类
        /// </summary>
        public AddParamConfigDefalutValue() : base(new P()) { }
    }
}
