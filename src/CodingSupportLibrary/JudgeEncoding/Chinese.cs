using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary.JudgeEncoding
{
    /// <summary>
    /// 支持编码: 中文编码类型
    /// </summary>
    internal class Chinese : IJudgeEncoding
    {
        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(FileInfo file)
        {
            byte[] datas = File.ReadAllBytes(file.FullName);
            return GetEncoding(datas);
        }

        /// <inheritdoc/>
        public JudgeEncodingResponse GetEncoding(byte[] contentBytes)
        {
            int suspected_chinese_byte_count = 0;

            // 是否确定不是中文
            bool is_confirm_not_chinese = false;
            for (int i = 0; i < contentBytes.Length; i++)
            {
                byte b = contentBytes[i];
                // 判断是 ASCII 基础编码
                if (b <= 0x7E)
                {
                    continue;
                }
                // 判断非 ASCII 编码 并且是最后一个没后续字节
                if (i == contentBytes.Length - 1)
                {
                    is_confirm_not_chinese = true;
                    break;
                }
                byte b2 = contentBytes[i + 1];
                // GBK 亦采用双字节表示
                // 总体编码范围为 8140-FEFE
                // 首字节在 81-FE 之间
                // 尾字节在 40-FE 之间
                // 剔除 xx7F 一条线。
                if (0x81 <= b && b <= 0xFE)
                {
                }
            }
        }
    }
}
