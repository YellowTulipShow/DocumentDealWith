using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 编码类型
    /// </summary>
    public enum ECodeType
    {
        /// <summary>
        /// UTF8 编码
        /// </summary>
        UTF8,

        /// <summary>
        /// UTF8 带 BOM 编码
        /// </summary>
        UTF8_BOM,

        /// <summary>
        /// ANSI 编码
        /// </summary>
        ANSI,
    }
}
