using System.CommandLine;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 选项注册
    /// </summary>
    public interface IOption
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        Option GetOption();
    }

    /// <summary>
    /// 接口: 选项注册
    /// </summary>
    public interface IOption<TParam> : IOption where TParam : IParameters
    {
        /// <summary>
        /// 是否全局
        /// </summary>
        bool IsGlobal();
        /// <summary>
        /// 填充参数
        /// </summary>
        void FillParam(InvocationContext context, TParam param);
    }
}
