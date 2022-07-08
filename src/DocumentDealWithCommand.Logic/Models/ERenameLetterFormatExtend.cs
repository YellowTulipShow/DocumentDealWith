using System;
using System.Linq;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 枚举使用扩展: 重命名使用字母组合配置
    /// </summary>
    public static class ERenameLetterFormatExtend
    {
        /// <summary>
        /// 获取枚举代表的字符组合
        /// </summary>
        /// <param name="eRenameLetterFormat">字母组合枚举</param>
        /// <returns>字符组合</returns>
        public static char[] ToChars(this ERenameLetterFormat eRenameLetterFormat)
        {
            return eRenameLetterFormat switch
            {
                ERenameLetterFormat.Lower => GetASCIIChars(97, 122),
                ERenameLetterFormat.Upper => GetASCIIChars(65, 90),
                ERenameLetterFormat.LowerAndUpper =>GetASCIIChars(97, 122).Concat(GetASCIIChars(65, 90)).ToArray(),
                ERenameLetterFormat.UpperAndLower => GetASCIIChars(65, 90).Concat(GetASCIIChars(97, 122)).ToArray(),
                _ => throw new ArgumentOutOfRangeException(nameof(eRenameLetterFormat),
                    $"重命名使用字母组合配置转为字符数组出错: {eRenameLetterFormat}"),
            };
        }

        /// <summary>
        /// ASCII 码指定范围获取对应的字符
        /// </summary>
        /// <param name="min">最小值(包含)</param>
        /// <param name="max">最大值(包含)</param>
        public static char[] GetASCIIChars(uint min, uint max)
        {
            const uint ascii_max = 127;
            if (min > ascii_max)
                throw new ArgumentOutOfRangeException(nameof(min), "最小值范围超过ASCII");
            if (max > ascii_max)
                throw new ArgumentOutOfRangeException(nameof(min), "最大值范围超过ASCII");
            if (min > max)
            {
                uint c = max;
                max = min;
                min = c;
            }
            uint len = max - min + 1;
            char[] rlist = new char[len];
            for (int i = 0; i < len && min <= max; i++, min++)
            {
                rlist[i] = (char)min;
            }
            return rlist;
        }
    }
}
