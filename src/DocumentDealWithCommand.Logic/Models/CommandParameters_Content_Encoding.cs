using CodingSupportLibrary;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 内容操作 - 更改编码
    /// </summary>
    public class CommandParameters_Content_Encoding : AbsBasicCommandParameters
    {
        /// <summary>
        /// 输出名称
        /// </summary>
        public ESupportEncoding Target { get; set; }
    }
}
