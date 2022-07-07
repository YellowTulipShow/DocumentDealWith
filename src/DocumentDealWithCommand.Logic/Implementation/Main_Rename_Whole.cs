﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 整体部分
    /// </summary>
    public class Main_Rename_Whole : AbsMain, IMain<ParamRenameWhole>
    {
        /// <inheritdoc/>
        public Main_Rename_Whole(ILog log) : base(log) { }


        private struct Result
        {
            public FileInfo Source { get; set; }
            public string NewName { get; set; }
        }
        /// <inheritdoc/>
        public int OnExecute(ParamRenameWhole param)
        {
            if (string.IsNullOrEmpty(param.NamingRules))
            {
                param.Print.WriteLine("命名规则不能为空!", EPrintColor.Red);
                return 2;
            }

            IList<Result> rlist = new List<Result>();
            param.Print.WriteLine($"预览:");
            for (uint i = 0; i < param.NeedHandleFileInventory.Length; i++)
            {
                FileInfo file = param.NeedHandleFileInventory[i];
                string newName = ToNewName(file.Name, param, i);
                rlist.Add(new Result()
                {
                    Source = file,
                    NewName = newName,
                });
            }
            if (param.IsPreview)
            {
                PrintPreview(param, rlist);
                return CheckPreviewOperationUserInput(param, rlist);
            }
            return ChangeFileName(param, rlist);
        }

        /// <summary>
        /// 转换文件名称
        /// </summary>
        /// <param name="fileName">文件名称(带扩展名)</param>
        /// <param name="param">转换参数</param>
        /// <param name="index">文件位置, 从0开始</param>
        /// <returns>新文件地址</returns>
        public string ToNewName(string fileName, ParamRenameWhole param, uint index)
        {
            // 分离文件名和扩展名, 如文件名: test1.jpg
            Match extension_match = Regex.Match(fileName, @"(\.[a-z0-9]+)$",
                RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            // 获取到扩展名, 如: .jpg
            string extension = extension_match.Success ? extension_match.Groups[1].Value : "";
            // 获取到文件名, 如: test1
            fileName = fileName.Replace(extension, "");
            // 判断更改扩展名
            if (param.IsChangeExtension)
                extension = param.ChangeExtensionValue;
            // 从0开始序号, 进行加一处理从1开始
            index += 1;
            // 乘以增量
            index *= param.Increment;
            // 增加起始数量
            index += param.StartedOnIndex;

            string rule = param.NamingRules;
            // 复制原名
            rule = rule.Replace("*", fileName);
            string code = index.ToString();
            // 判断补位
            if (param.IsInsufficientSupplementBit)
            {
                uint totalWidth = Math.Max((uint)code.Length, param.Digit);
                code = code.PadLeft((int)totalWidth, '0');
            }
            rule = rule.Replace("#", code);
            return $"{rule}{extension}";
        }

        private string ToShowFileName(FileInfo info, DirectoryInfo rootDire)
        {
            string name = info.FullName.Replace(rootDire.FullName, ".");
            name = name.Replace('\\', '/').Replace(":", "");
            if (Regex.IsMatch(name, @"^[a-z]/", RegexOptions.ECMAScript | RegexOptions.IgnoreCase))
                return $"/{name}";
            return name;
        }

        private void PrintPreview(ParamRenameWhole param, IList<Result> rlist)
        {
            param.Print.WriteLine("文件名称转换预览:\n");
            for (int i = 0; i < rlist.Count; i++)
            {
                Result r = rlist[i];
                string showName = ToShowFileName(r.Source, param.RootDire);
                param.Print.WriteLine($"[{i + 1}]: {showName} => {r.NewName}");
            }
        }

        private int CheckPreviewOperationUserInput(ParamRenameWhole param, IList<Result> rlist)
        {
            do
            {
                param.Print.WriteLine($"\n请按提示输入 [ ] 内容标识用于后续操作:");
                param.Print.WriteLine($"[0]: 开始重命名, 无需做额外操作");
                param.Print.WriteLine($"[1]: 在队列调整文件的位置");
                param.Print.WriteLine($"[2]: 在队列中删除目标文件");
                param.Print.WriteLine($"[q]: 终止重命名操作");
                int input = Console.Read();
                param.Print.WriteLine($"您输入的数字: [{input}]");
                if ('0' == input)
                {
                    return ChangeFileName(param, rlist);
                }
                else if ('1' == input)
                {
                    return MoveFilePosition(param, rlist);
                }
                else if ('2' == input)
                {
                    return MoveFilePosition(param, rlist);
                }
                else if ('q' == input)
                {
                    param.Print.WriteLine("您选择了终止操作, 已退出操作");
                    return 0;
                }
                char.TryParse(input.ToString(), out char c);
                param.Print.Write("未识别到您的输入内容: ");
                param.Print.WriteLine(c.ToString(), EPrintColor.Red);
            } while (true);
        }

        private int ChangeFileName(ParamRenameWhole param, IList<Result> rlist)
        {
            param.Print.WriteLine("未实现更改文件名称逻辑", EPrintColor.Red);
            return 2;
        }

        private int MoveFilePosition(ParamRenameWhole param, IList<Result> rlist)
        {
            param.Print.WriteLine($"\n请输入表达式, 指定您要移动的项与位置:");
            param.Print.WriteLine($"[0]: 开始重命名, 无需做额外操作");
        }

        private int DeleteFile(ParamRenameWhole param, IList<Result> rlist)
        {
        }
    }
}
