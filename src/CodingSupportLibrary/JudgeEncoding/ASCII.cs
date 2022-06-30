using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: ASCII 基础编码
    /// </summary>
    internal class ASCII : IJudgeEncoding
    {
        /// <summary>
        /// 字节最大值
        /// </summary>
        public const byte MAX = 127; // 0x7F;

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] contentBytes)
        {
            var response = new JudgeEncodingResponse()
            {
                Encoding = null,
                ContentBytes = contentBytes,
                IsReadFileALLContent = true,
            };
            for (int i = 0; i < contentBytes.Length; i++)
            {
                byte b = contentBytes[i];
                // 判断是 ASCII 基础编码
                if (b > MAX)
                {
                    return response;
                }
            }
            response.Encoding = Encoding.ASCII;
            return response;
        }
    }
}
