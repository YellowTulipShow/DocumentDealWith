using System;

using System.CommandLine;
using System.CommandLine.Invocation;

using DocumentDealWithCommand.Logic;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 接口: 选项注册
    /// </summary>
    public interface IOptionRegistration<TParam> where TParam : ICommandParameters, new()
    {
        /// <summary>
        /// 获取选项
        /// </summary>
        Option GetOption();

        /// <summary>
        /// 是否是全局选项
        /// </summary>
        bool IsGlobalOption();

        /// <summary>
        /// 填充参数
        /// </summary>
        void FillParam(InvocationContext context, TParam param);
    }

    /// <summary>
    /// 配置项注册
    /// </summary>
    /// <typeparam name="TOptionValue">配置项值类型</typeparam>
    /// <typeparam name="TParam">参数对象</typeparam>
    public struct OptionRegistration<TOptionValue, TParam> : IOptionRegistration<TParam> where TParam : ICommandParameters, new()
    {
        private readonly Option<TOptionValue> option;
        private readonly bool isGlobal;
        private readonly Action<TParam, TOptionValue> fillParamMethod;

        /// <summary>
        /// 实例化配置项注册
        /// </summary>
        /// <param name="option">配置项</param>
        /// <param name="fillParamMethod">填充参数方法</param>
        /// <param name="isGlobal">是否全局</param>
        public OptionRegistration(Option<TOptionValue> option, Action<TParam, TOptionValue> fillParamMethod, bool isGlobal = false)
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

        /// <summary>
        /// 是否是全局选项
        /// </summary>
        public bool IsGlobalOption()
        {
            return isGlobal;
        }

        /// <inheritdoc/>
        public void FillParam(InvocationContext context, TParam param)
        {
            TOptionValue value = context.ParseResult.GetValueForOption(option);
            fillParamMethod.Invoke(param, value);
        }
    }
}
