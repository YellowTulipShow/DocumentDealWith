namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 文件名称部分分离结果
    /// </summary>
    public struct MFileName
    {
        /// <summary>
        /// 名称部分
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
    }
}
