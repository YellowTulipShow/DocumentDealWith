using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;

using YTS.Log;

namespace CommandParamUse
{
    /// <summary>
    /// 基础的实现执行方案
    /// </summary>
    /// <typeparam name="TParam">参数对象</typeparam>
    public abstract class AbsExecute<TParam> : IExecute<TParam> where TParam : IParameters
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;

        /// <summary>
        /// 实例化执行方案
        /// </summary>
        /// <param name="log">日志接口</param>
        public AbsExecute(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 获取泛型命令输出参数各项
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IInputOption<TParam>> GetInputs();
        /// <inheritdoc/>
        public abstract bool IsExecute();
        /// <inheritdoc/>
        public abstract TParam GetParam();
        /// <inheritdoc/>
        public abstract int OnExecute(InvocationContext context, TParam param);
    }
}
