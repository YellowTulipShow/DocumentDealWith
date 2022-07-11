using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 处理结果集合
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <typeparam name="TResult">数据转化结果</typeparam>
    public class HandleResult<TData, TResult>
    {
        /// <summary>
        /// 数据原值
        /// </summary>
        public TData Source { get; set; }
        /// <summary>
        /// 结果内容
        /// </summary>
        public TResult Result { get; set; }
    }
}
