using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 命令输入
    /// </summary>
    public interface ICommandInput
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        Option GetOption();
    }

    /// <summary>
    /// 接口: 指定参数类型的命令输入
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    public interface ICommandInput<TParam> : ICommandInput where TParam : IParameters
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

    /// <summary>
    /// 命令输入
    /// </summary>
    /// <typeparam name="TOptionValue">配置项值类型</typeparam>
    /// <typeparam name="TParam">参数对象</typeparam>
    public struct CommandInput<TOptionValue, TParam> : ICommandInput<TParam> where TParam : IParameters
    {
        private readonly Option<TOptionValue> option;
        private readonly bool isGlobal;
        private readonly Action<TParam, TOptionValue> fillParamMethod;

        /// <summary>
        /// 实例化命令输入
        /// </summary>
        /// <param name="option">配置项</param>
        /// <param name="fillParamMethod">填充参数方法</param>
        /// <param name="isGlobal">是否全局</param>
        public CommandInput(Option<TOptionValue> option, Action<TParam, TOptionValue> fillParamMethod, bool isGlobal = false)
        {
            this.option = option;
            this.fillParamMethod = fillParamMethod;
            this.isGlobal = isGlobal;
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
            fillParamMethod.Invoke(param, value);
        }

        /// <inheritdoc/>
        public bool IsGlobal()
        {
            return isGlobal; ;
        }
    }
}
