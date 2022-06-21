
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic
{
    /// <summary>
    /// 接口: 主逻辑执行
    /// </summary>
    public interface IMain<T> where T : AbsBasicCommandParameters, new()
    {
        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="commandParameters">命令参数</param>
        void OnExecute(T commandParameters);
    }
}
