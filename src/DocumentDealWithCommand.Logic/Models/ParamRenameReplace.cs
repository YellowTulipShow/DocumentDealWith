namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 重命名 - 替换
    /// </summary>
    public class ParamRenameReplace : AbsParamRename
    {
        /// <summary>
        /// 匹配项, 支持正则表达式
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 替换内容
        /// </summary>
        public string Replacement { get; set; }
    }
}
