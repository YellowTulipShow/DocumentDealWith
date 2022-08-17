using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CommandParamUse.Implementation
{
    /// <summary>
    /// 命令输入
    /// </summary>
    /// <typeparam name="TOptionValue">配置项值类型</typeparam>
    /// <typeparam name="TParam">参数对象</typeparam>
    public class InputOption<TOptionValue, TParam> : IInputOption<TParam>
    {
        private readonly Option<TOptionValue> option;
        private readonly Action<TParam, TOptionValue> fillParamMethod;

        /// <summary>
        /// 实例化命令输入
        /// </summary>
        /// <param name="option">配置项</param>
        /// <param name="fillParamMethod">填充参数方法</param>
        public InputOption(Option<TOptionValue> option, Action<TParam, TOptionValue> fillParamMethod)
        {
            this.option = option;
            this.fillParamMethod = fillParamMethod;
        }

        /// <inheritdoc/>
        public Option GetOption()
        {
            return option;
        }

        /// <inheritdoc/>
        public void FillParam(InvocationContext context, TParam param)
        {
            TOptionValue value = context.ParseResult.GetValueForOption(option);
            if (fillParamMethod != null)
            {
                fillParamMethod.Invoke(param, value);
            }
        }
    }
}
