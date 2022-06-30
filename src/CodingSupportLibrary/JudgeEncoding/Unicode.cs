using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: Unicode 类型
    /// </summary>
    internal class Unicode : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            using FileStream fs = file.Open(FileMode.Open, FileAccess.Read);
            using BinaryReader br = new BinaryReader(fs);
            byte[] buffer = br.ReadBytes(4);
            var response = new JudgeEncodingResponse()
            {
                IsReadFileALLContent = false,
                Encoding = JudgeHeader(buffer),
                ContentBytes = null,
            };
            return response;
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
        private IEnumerable<(byte[] bom, Func<Encoding> getFunc)> HeaderRule()
        {
            yield return (new byte[] { 0xEF, 0xBB, 0xBF }, () => new UTF8Encoding(true));
            // 可能不在头部中
            yield return (new byte[] { 0x2B, 0x2F, 0x76 }, () => Encoding.UTF7);
            yield return (new byte[] { 0xE6, 0xB1, 0x89 }, () => new UTF8Encoding(false));
            // 混乱
            // UTF-16 格式
            yield return (new byte[] { 0xFE, 0xFF }, () => Encoding.BigEndianUnicode);
            yield return (new byte[] { 0xFF, 0xFE }, () => Encoding.Unicode);
            // UTF-32 格式
            yield return (new byte[] { 0, 0, 0xFE, 0xFF }, () => Encoding.UTF32);
        }
    }
}
