using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: Unicode 头部判断 类型
    /// </summary>
    internal class UnicodeHeader : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                using BinaryReader br = new BinaryReader(fs);
                byte[] buffer = br.ReadBytes(4);
                return new JudgeEncodingResponse()
                {
                    IsReadFileALLContent = false,
                    Encoding = JudgeHeader(buffer),
                    ContentBytes = null,
                };
            }
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] contentBytes)
        {
            var response = new JudgeEncodingResponse()
            {
                IsReadFileALLContent = true,
                Encoding = JudgeHeader(contentBytes),
                ContentBytes = contentBytes,
            };
            return response;
        }

        private Encoding JudgeHeader(byte[] buffer)
        {
            if (buffer == null || buffer.Length <= 0)
            {
                return null;
            }
            static IEnumerable<(byte[] bom, Func<Encoding> getFunc)> HeaderRule()
            {
                // UTF-32 格式
                yield return (new byte[] { 0xFF, 0xFE, 0x00, 0x00 }, () => Encoding.UTF32);
                // UTF-8 BOM 格式头部必带标识
                yield return (new byte[] { 0xEF, 0xBB, 0xBF }, () => new UTF8Encoding(true));
                // UTF-16 格式 标识
                yield return (new byte[] { 0xFE, 0xFF }, () => Encoding.BigEndianUnicode);
                yield return (new byte[] { 0xFF, 0xFE }, () => Encoding.Unicode);
            };
            // 判断头部字符
            foreach (var (bom, getFunc) in HeaderRule())
            {
                if (buffer.Length < bom.Length)
                    continue;
                bool is_all_equal = true;
                for (int i = 0; i < bom.Length; i++)
                {
                    if (buffer[i] != bom[i])
                    {
                        is_all_equal = false;
                        break;
                    }
                }
                if (is_all_equal)
                {
                    return getFunc();
                }
            }
            return null;
        }
    }
}
