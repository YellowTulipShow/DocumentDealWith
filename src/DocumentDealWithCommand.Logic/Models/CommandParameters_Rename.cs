namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 重命名
    /// </summary>
    public class CommandParameters_Rename : AbsBasicCommandParameters
    {
        /// <summary>
        /// 输出名称
        /// </summary>
        public string OutputName { get; set; }
    }
}
