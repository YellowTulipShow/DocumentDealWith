namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 重命名 - 整体
    /// </summary>
    public class ParamRenameWhole : AbsParamRename
    {
        /// <summary>
        /// 命名规则
        /// </summary>
        public string NamingRules { get; set; }
        /// <summary>
        /// 开始于
        /// </summary>
        public uint StartedOnIndex { get; set; }
        /// <summary>
        ///增量
        /// </summary>
        public uint Increment { get; set; }
        /// <summary>
        /// 位数
        /// </summary>
        public uint Digit { get; set; }

        /// <summary>
        /// 是否不足位补齐
        /// </summary>
        public bool IsInsufficientSupplementBit { get; set; }

        /// <summary>
        /// 是否使用字母
        /// </summary>
        public bool IsUseLetter { get; set; }

        /// <summary>
        /// 使用字母类型格式
        /// </summary>
        public ERenameLetterFormat UseLetterFormat { get; set; }

        /// <summary>
        /// 是否更改扩展名
        /// </summary>
        public bool IsChangeExtension { get; set; }

        /// <summary>
        /// 更改扩展名内容
        /// </summary>
        public string ChangeExtensionValue { get; set; }
    }
}
