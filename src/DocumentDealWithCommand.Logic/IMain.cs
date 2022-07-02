
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic
{
    /// <summary>
    /// 接口: 主逻辑执行
    /// </summary>
    public interface IMain<T> where T : ICommandParameters, new()
    {
        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>推出代码</returns>
        int OnExecute(T commandParameters);
    }
}
