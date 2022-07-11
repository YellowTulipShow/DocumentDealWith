namespace DocumentDealWithCommand.Logic
{
    /// <summary>
    /// 处理数据
    /// </summary>
    /// <typeparam name="TData">数据类型</typeparam>
    /// <typeparam name="TResult">数据转化结果</typeparam>
    public interface IHandleData<TData, TResult>
    {
        /// <summary>
        /// 数据转为结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="index">序列号, 从0开始</param>
        /// <returns>结果</returns>
        TResult ToResult(TData data, int index);

        /// <summary>
        /// 转为打印数据的字符串输出
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>数据的字符串输出</returns>
        string ToPrint(TData data);
    }
}
