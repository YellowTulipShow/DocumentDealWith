using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 换行符类型
    /// </summary>
    public enum ENewLineType
    {
        /// <summary>
        /// Window 默认类型 \r\n
        /// </summary>
        WindowRN,

        /// <summary>
        /// Unix 类型 \n
        /// </summary>
        Unix,
    }
}
