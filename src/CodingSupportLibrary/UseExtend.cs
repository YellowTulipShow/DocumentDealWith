using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary
{
    /// <summary>
    /// 静态调用扩展方法
    /// </summary>
    public static class UseExtend
    {
        /// <summary>
        /// 注册支持代码页
        /// </summary>
        public static void SupportCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 判断得到文件的编码内容
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <returns></returns>
        public static Encoding GetEncoding(this FileInfo file)
        {
            byte[] contentBytes = null;
            foreach (var item in GetJudgeEncodings())
            {
                bool isNullContentBytes = contentBytes == null || contentBytes.Length <= 0;
                JudgeEncodingResponse response = isNullContentBytes ?
                    item.GetEncoding(file) :
                    item.GetEncoding(contentBytes);
                if (response.Encoding != null)
                    return response.Encoding;

                if (response.IsReadFileALLContent)
                    contentBytes = response.ContentBytes;
            }
            return null;
        }
        private static IEnumerable<IJudgeEncoding> GetJudgeEncodings()
        {
            yield return new JudgeEncoding.UnicodeHeader();
            yield return new JudgeEncoding.ASCII();
            yield return new JudgeEncoding.UTF8NoBOM();
            yield return new JudgeEncoding.Chinese();
        }
    }
}
