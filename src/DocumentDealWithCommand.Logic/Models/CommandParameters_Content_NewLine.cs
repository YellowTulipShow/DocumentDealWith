namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 内容操作 - 更改编码
    /// </summary>
    public class CommandParameters_Content_NewLine : BasicCommandParameters
    {
        /// <summary>
        /// 换行符类型
        /// </summary>
        public ENewLineType Type { get; set; }
    }
}
