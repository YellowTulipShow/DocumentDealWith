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
        /// <summary>
        /// ASCII 编码字节最大值
        /// </summary>
        public const byte ASCII_MAX = 127; // 0x7F;

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            var response = new JudgeEncodingResponse()
            {
                IsReadFileALLContent = false,
                Encoding = null,
                ContentBytes = null,
            };
            using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
            {
                using BinaryReader br = new BinaryReader(fs);
                byte[] buffer = br.ReadBytes(4);
                response.Encoding = JudgeHeader(buffer);
            }
            if (response.Encoding == null)
            {
                response.IsReadFileALLContent = true;
                response.ContentBytes = File.ReadAllBytes(file.FullName);
                response.Encoding = JudgeUTF8NoBOM(response.ContentBytes);
            }
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
            if (response.Encoding == null)
            {
                response.Encoding = JudgeUTF8NoBOM(contentBytes);
            }
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

        private Encoding JudgeUTF8NoBOM(byte[] buffer)
        {
            const uint rank_item_left = 0b_1000_0000;
            const uint rank_item_right = 0b_1100_0000;
            uint[] ranks = {
                0b_1100_0000,
                0b_1110_0000,
                0b_1111_0000,
                0b_1111_1000,
                0b_1111_1100,
                0b_1111_1110,
            };
            // 获取后续字节数量
            int get_follow_byte_count(byte b)
            {
                for (int rankIndex = 1; rankIndex < ranks.Length; rankIndex++)
                {
                    uint l = ranks[rankIndex - 1];
                    uint r = ranks[rankIndex];
                    // 在指定的范围内
                    if (l <= b && b < r)
                    {
                        return rankIndex;
                    }
                }
                return -1;
            };

            int suspected_ASCII_byte_count = 0;
            int suspected_UTF8_byte_count = 0;
            int bufferIndex = 0;
            while (bufferIndex < buffer.Length)
            {
                byte b = buffer[bufferIndex];
                // 属于 ASCII 单字节内容
                if (b <= ASCII_MAX)
                {
                    suspected_ASCII_byte_count++;
                    bufferIndex++;
                    continue;
                }
                int follow_byte_count = get_follow_byte_count(b);
                // 不符合 UTF8 编码规则: 第一个字节表示: 字节总数规则
                if (follow_byte_count <= 0)
                    return null;
                suspected_UTF8_byte_count++;
                while (follow_byte_count > 0)
                {
                    bufferIndex++;
                    // 超出范围也判定无法识别规则
                    if (bufferIndex >= buffer.Length)
                        return null;
                    b = buffer[bufferIndex];
                    if (rank_item_left <= b && b < rank_item_right)
                    {
                        suspected_UTF8_byte_count++;
                        follow_byte_count--;
                        continue;
                    }
                    // 不符合 UTF8 编码规则: 后续字节符合规范 10xx xxxx
                    return null;
                }
                bufferIndex++;
            }
            // 总字节数 == ASCII单字节数 + UTF8组合字节组数 即可判断是 UTF8 无BOM 编码格式
            if (buffer.Length == suspected_ASCII_byte_count + suspected_UTF8_byte_count)
            {
                return new UTF8Encoding(false);
            }
            // 表示无法识别为 UTF8 无BOM 编码
            return null;
        }
    }
}
