namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 换行符类型
    /// </summary>
    public enum ENewLineType
    {
        /// <summary>
        /// 换行: \n | 0x0A
        /// </summary>
        LF = 0x0A,
        /// <summary>
        /// 回车: \r | 0x0D
        /// </summary>
        CR = 0x0D,
        /// <summary>
        /// 回车+换行: \r\n | 0x0D 0x0A
        /// </summary>
        CRLF = 0x0D + 0x0A,
    }
}
