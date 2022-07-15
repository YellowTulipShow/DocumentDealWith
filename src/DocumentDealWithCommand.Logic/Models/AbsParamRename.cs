namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 重命名 - 通用项
    /// </summary>
    public abstract class AbsParamRename : BasicCommandParameters
    {
        /// <summary>
        /// 是否预览
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// 预览展示的列数
        /// </summary>
        public uint PreviewColumnCount { get; set; }
    }
}
