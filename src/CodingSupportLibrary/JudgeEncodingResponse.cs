using System.Text;

namespace CodingSupportLibrary
{
    /// <summary>
    /// 判断编码响应结果
    /// </summary>
    internal struct JudgeEncodingResponse
    {
        /// <summary>
        /// 判断的文件编码内容
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 是否已经读取文件全部字节
        /// </summary>
        public bool IsReadFileALLContent { get; set; }
        /// <summary>
        /// 文件内容字节数组
        /// </summary>
        public byte[] ContentBytes { get; set; }
    }
}
