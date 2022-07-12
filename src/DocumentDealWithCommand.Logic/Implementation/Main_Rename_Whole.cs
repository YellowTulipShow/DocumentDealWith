using System;
using System.IO;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{

    /// <summary>
    /// 实现命令类: 重命名 - 整体部分
    /// </summary>
    public class Main_Rename_Whole : AbsMainReanme<ParamRenameWhole>
    {
        /// <inheritdoc/>
        public Main_Rename_Whole(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override int OnExecute(ParamRenameWhole param)
        {
            if (string.IsNullOrEmpty(param.NamingRules))
            {
                param.Print.WriteLine("命名规则不能为空!", EPrintColor.Red);
                return 2;
            }
            if (param.Increment <= 0)
            {
                param.Print.WriteLine("增量必须大于0!", EPrintColor.Red);
                return 2;
            }
            return base.OnExecute(param);
        }

        /// <inheritdoc/>
        public override string ToResult(FileInfo data, int index)
        {
            return ToNewName(data.Name, (uint)index);
        }

        /// <summary>
        /// 转换文件名称
        /// </summary>
        /// <param name="fileName">文件名称(带扩展名)</param>
        /// <param name="index">文件位置, 从0开始</param>
        /// <returns>新文件地址</returns>
        public string ToNewName(string fileName, uint index)
        {
            var mfile = FileInfoExtend.ToMFileName(fileName);
            // 判断更改扩展名
            if (param.IsChangeExtension)
                mfile.Extension = param.ChangeExtensionValue;

            int number = (int)index;
            number = number * (int)param.Increment + (int)param.StartedOnIndex;

            string rule = param.NamingRules;
            // 复制原名
            rule = rule.Replace("*", mfile.Name);
            //
            string code = number.ToString();
            if (param.IsUseLetter)
                code = CalcLetterCode(number, param.UseLetterFormat);
            // 判断补位
            if (param.IsInsufficientSupplementBit)
            {
                uint totalWidth = Math.Max((uint)code.Length, param.Digit);
                code = code.PadLeft((int)totalWidth, '0');
            }
            rule = rule.Replace("#", code);
            return $"{rule}{mfile.Extension}";
        }

        /// <summary>
        /// 计算字母组合编码结果
        /// </summary>
        /// <param name="number">从1开始</param>
        /// <param name="UseLetterFormat">使用字母编码格式</param>
        /// <returns>结果</returns>
        public string CalcLetterCode(int number, ERenameLetterFormat UseLetterFormat)
        {
            if (number <= 0)
                return string.Empty;
            char[] letters = UseLetterFormat.ToChars();
            int len = letters.Length;
            // 倍数
            int multiple = number / len;
            // 整数
            int remainder = number % len;
            if (remainder == 0)
            {
                remainder = len;
                if (multiple > 0)
                    multiple--;
            }
            remainder--;
            char letter = letters[remainder];
            if (multiple == 0)
                return letter.ToString();
            string prefix = CalcLetterCode(multiple, UseLetterFormat);
            return $"{prefix}{letter}";
        }
    }
}
