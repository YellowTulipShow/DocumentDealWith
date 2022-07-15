namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 命令参数: 重命名 - 添加/删除
    /// </summary>
    public class ParamRenameAddOrDelete : AbsParamRename
    {
        /// <summary>
        /// 文件名前添加
        /// </summary>
        public string BeforeAdd { get; set; }

        /// <summary>
        /// 文件名后添加
        /// </summary>
        public string AfterAdd { get; set; }

        /// <summary>
        /// 是否使用扩展添加
        /// </summary>
        public bool IsUseExtendAdd { get; set; }

        /// <summary>
        /// 从文件名第 `N` 个字符之后开始添加
        /// </summary>
        public uint UseExtendAddStartCharIndex { get; set; }

        /// <summary>
        /// 扩展添加内容
        /// </summary>
        public string UseExtendAddContent { get; set; }

        /// <summary>
        /// 删除内容部分, 支持正则表达式
        /// </summary>
        public string DeleteContent { get; set; }

        /// <summary>
        /// 是否使用扩展删除
        /// </summary>
        public bool IsUseExtendDelete { get; set; }

        /// <summary>
        /// 从文件名第 `N` 个字符开始删除
        /// </summary>
        public uint UseExtendDeleteStartCharIndex { get; set; }

        /// <summary>
        /// 启用扩展删除, 需要删除的字符数量
        /// </summary>
        public uint UseExtendDeleteCount { get; set; }
    }
}
