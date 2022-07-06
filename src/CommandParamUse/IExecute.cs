using System.Collections.Generic;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 执行方案
    /// </summary>
    public interface IExecute
    {
        /// <summary>
        /// 获取需全局配置的命令行输入项
        /// </summary>
        IEnumerable<IInputOption> GetGlobalInputs();

        /// <summary>
        /// 获取命令行输入项
        /// </summary>
        IEnumerable<IInputOption> GetInputs();

        /// <summary>
        /// 是否需要执行逻辑
        /// </summary>
        bool IsExecute();

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="context">命令行解析结果上下文</param>
        /// <returns>执行命令返回结果数字标识</returns>
        int OnExecute(InvocationContext context);
    }

    /// <summary>
    /// 接口: 执行方案带有明确参数的执行方案独享
    /// </summary>
    /// <typeparam name="TParam">参数</typeparam>
    public interface IExecute<TParam> : IExecute where TParam : IParameters
    {
        /// <summary>
        /// 获取命令行输入项
        /// </summary>
        new IEnumerable<IInputOption<TParam>> GetInputs();
        IEnumerable<IInputOption> IExecute.GetGlobalInputs()
        {
            foreach (var item in GetInputs() ?? new IInputOption<TParam>[] { })
                if (item.IsGlobal())
                    yield return item;
        }
        IEnumerable<IInputOption> IExecute.GetInputs()
        {
            foreach (var item in GetInputs() ?? new IInputOption<TParam>[] { })
                if (!item.IsGlobal())
                    yield return item;
        }

        /// <summary>
        /// 获取基础参数
        /// </summary>
        /// <returns>参数对象</returns>
        TParam GetParam();

        /// <summary>
        /// 根据泛型参数执行逻辑
        /// </summary>
        /// <param name="context">命令行解析结果上下文</param>
        /// <param name="param">泛型参数</param>
        /// <returns>执行命令返回结果数字标识</returns>
        int OnExecute(InvocationContext context, TParam param);
        int IExecute.OnExecute(InvocationContext context)
        {
            TParam param = GetParam();
            foreach (var item in GetInputs() ?? new IInputOption<TParam>[] { })
            {
                item.FillParam(context, param);
            }
            return OnExecute(context, param);
        }
    }
}
