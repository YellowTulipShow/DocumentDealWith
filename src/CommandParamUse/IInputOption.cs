using System.CommandLine;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 指定参数类型的命令输入
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public interface IInputOption<TParam>
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        Option GetOption();

        /// <summary>
        /// 填充参数
        /// </summary>
        void FillParam(InvocationContext context, TParam param);
    }
}
