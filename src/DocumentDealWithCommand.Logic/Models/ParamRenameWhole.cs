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
    }

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
        public int StartedOnIndex { get; set; }
        /// <summary>
        ///增量
        /// </summary>
        public int Increment { get; set; }
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

        /// <summary>
        /// 是否自动解决重命名冲突
        /// </summary>
        public bool IsAutomaticallyResolveRenameConflicts { get; set; }
    }

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
