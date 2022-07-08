using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 重命名使用字母组合配置
    /// </summary>
    public enum ERenameLetterFormat
    {
        /// <summary>
        /// 小写
        /// </summary>
        Lower,
        /// <summary>
        /// 大写
        /// </summary>
        Upper,
        /// <summary>
        /// 小写+大写组合
        /// </summary>
        LowerAndUpper,
        /// <summary>
        /// 大写+小写组合
        /// </summary>
        UpperAndLower,
    }
}
