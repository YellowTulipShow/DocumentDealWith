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

        IEnumerable<IInput> IExecute.GetGlobalInputs()
        {
            foreach (var item in GetInputs() ?? new IInput<TParam>[] { })
                if (item.IsGlobal())
                    yield return item;
        }
        IEnumerable<IInput> IExecute.GetInputs()
        {
            foreach (var item in GetInputs() ?? new IInput<TParam>[] { })
                if (!item.IsGlobal())
                    yield return item;
        }

        /// <inheritdoc/>
        public virtual int OnExecute(InvocationContext context)
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                TParam param = GetParam();
                foreach (var item in GetInputs() ?? new IInput<TParam>[] { })
                {
                    item.FillParam(context, param);
                }
                param.WriteLogArgs(logArgs);
                return OnExecute(context, param);
            }
            catch (Exception ex)
            {
                log.Error("执行逻辑出错!", ex, logArgs);
                throw ex;
            }
        }

        /// <summary>
        /// 获取泛型命令输出参数各项
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IInput<TParam>> GetInputs();
        /// <inheritdoc/>
        public abstract bool IsExecute();
        /// <inheritdoc/>
        public abstract TParam GetParam();
        /// <inheritdoc/>
        public abstract int OnExecute(InvocationContext context, TParam param);
    }
}
