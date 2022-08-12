using System.Collections.Generic;
using System.CommandLine.Invocation;

namespace CommandParamUse
{
    /// <summary>
    /// 接口: 执行方案带有明确参数的执行方案独享
    /// </summary>
    /// <typeparam name="P">参数</typeparam>
    public interface IExecute<P> where P : IParam
    {
        int OnExecute(P param);
    }
}
