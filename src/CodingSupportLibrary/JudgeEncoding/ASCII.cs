﻿using System.IO;
using System.Text;

namespace CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: ASCII 基础编码
    /// </summary>
    internal class ASCII : IJudgeEncoding
    {
        /// <summary>
        /// ASCII 编码字节最大值
        /// </summary>
        public const byte MAX = 127; // 0x7F;

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            byte[] datas = File.ReadAllBytes(file.FullName);
            return GetEncoding(datas);
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] buffer)
        {
            JudgeEncodingResponse response = new JudgeEncodingResponse()
            {
                Encoding = JudgeASCII(buffer),
                ContentBytes = buffer,
                IsReadFileALLContent = true,
            };
            return response;
        }

        private Encoding JudgeASCII(byte[] buffer)
        {
            int bufferIndex = 0;
            while (bufferIndex < buffer.Length)
            {
                byte b = buffer[bufferIndex];
                // 不属于 ASCII 单字节内容
                if (b > MAX)
                {
                    return null;
                }
                bufferIndex++;
            }
            return Encoding.ASCII;
        }
    }
}
