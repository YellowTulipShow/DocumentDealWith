using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary
{
    /// <summary>
    /// 静态调用扩展测试方法
    /// </summary>
    public static class UseTestExtend
    {
        public static string GetWriteLogFileContentBytesContent(this FileInfo codeFile)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine($"文件: {codeFile.FullName}");
            byte[] datas = File.ReadAllBytes(codeFile.FullName);
            str.AppendLine($"名称: {codeFile.Name}");

            str.AppendLine($"HEX 十六进制:");
            str.Append(WriteLogFileContentBytes_HEX(datas));
            str.AppendLine($"OCT 十进制:");
            str.Append(WriteLogFileContentBytes_OCT(datas));

            return str.ToString();
        }
        private static StringBuilder WriteLogFileContentBytes_HEX(byte[] datas)
        {
            StringBuilder str = new StringBuilder();
            const char space = ' ';
            const int region = 20;
            int header_len = (datas.Length + region).ToString().Length;
            string get_line_header(int line)
            {
                return $"  {line.ToString().PadLeft(header_len, space)}:  ";
            };
            str.Append($"{"".PadLeft(get_line_header(0).Length)}");
            string item_space = "".PadLeft(2, space);
            for (int i = 0; i < region; i++)
            {
                str.Append($"{i,3}{item_space}");
            }
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % region == 0)
                {
                    int line = (int)Math.Ceiling((decimal)(i / region)) * region;
                    str.Append($"\n{get_line_header(line)}");
                }
                str.Append($"{datas[i],3:X2}{item_space}");
            }
            str.AppendLine(string.Empty);
            return str;
        }
        private static StringBuilder WriteLogFileContentBytes_OCT(byte[] datas)
        {
            StringBuilder str = new StringBuilder();
            const char space = ' ';
            const int region = 20;
            int header_len = (datas.Length + region).ToString().Length;
            string get_line_header(int line)
            {
                return $"  {line.ToString().PadLeft(header_len, space)}:  ";
            };
            str.Append($"{"".PadLeft(get_line_header(0).Length)}");
            string item_space = "".PadLeft(2, space);
            for (int i = 0; i < region; i++)
            {
                str.Append($"{i,3}{item_space}");
            }
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % region == 0)
                {
                    int line = (int)Math.Ceiling((decimal)(i / region)) * region;
                    str.Append($"\n{get_line_header(line)}");
                }
                str.Append($"{datas[i],3}{item_space}");
            }
            str.AppendLine(string.Empty);
            return str;
        }
    }
}
