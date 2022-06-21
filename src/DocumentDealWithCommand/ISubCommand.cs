using System.CommandLine;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 接口: 子命令
    /// </summary>
    public interface ISubCommand
    {
        /// <summary>
        /// 获取子命令对象
        /// </summary>
        /// <returns>子命令</returns>
        Command GetCommand();
    }
}
