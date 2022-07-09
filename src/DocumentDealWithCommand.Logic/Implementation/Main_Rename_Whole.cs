using System;
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
            if (param.Increment <= 0)
            {
                param.Print.WriteLine("增量必须大于0!", EPrintColor.Red);
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

            int number = (int)index;
            number = number * (int)param.Increment + (int)param.StartedOnIndex;

            string rule = param.NamingRules;
            // 复制原名
            rule = rule.Replace("*", fileName);
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
            return $"{rule}{extension}";
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
                    return MoveFilePosition(param);
                }
                else if ('2' == input)
                {
                    return DeleteFile(param);
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
            var logArgs = log.CreateArgDictionary();
            IDictionary<string, Result> temporaryNameDict = new Dictionary<string, Result>();
            logArgs["执行部分"] = "非临时文件名称替代执行";
            for (int i = 0; i < rlist.Count; i++)
            {
                try
                {
                    var m = rlist[i];
                    logArgs["m.Source.FullName"] = m.Source.FullName;
                    logArgs["m.NewName"] = m.NewName;
                    DirectoryInfo dire = m.Source.Directory;
                    string newFilePath = Path.Combine(dire.FullName, m.NewName);
                    FileInfo newFile = new FileInfo(newFilePath);
                    if (newFile.Exists)
                    {
                        string temporaryNamePath = CalcTemporaryName(m.Source, i);
                        temporaryNameDict[temporaryNamePath] = m;
                        m.Source.MoveTo(temporaryNamePath);
                        continue;
                    }
                    m.Source.MoveTo(newFile.FullName);
                    param.Print.WriteLine($"重命名: {ToShowFileName(m.Source, param.RootDire)} => {m.NewName}");
                }
                catch (Exception ex)
                {
                    log.Error("非临时文件名称替代执行", ex, logArgs);
                    continue;
                }
            }
            logArgs["执行部分"] = "临时文件相关重命名";
            foreach (string temporaryNamePath in temporaryNameDict.Keys)
            {
                try
                {
                    var m = temporaryNameDict[temporaryNamePath];
                    logArgs["m.Source.FullName"] = m.Source.FullName;
                    logArgs["m.NewName"] = m.NewName;
                    DirectoryInfo dire = m.Source.Directory;
                    string newFilePath;
                    do
                    {
                        newFilePath = Path.Combine(dire.FullName, m.NewName);
                        if (!File.Exists(newFilePath))
                            break;
                        m.NewName = Regex.Replace(m.NewName, @"^(.*)\.([a-z0-9])$", @"$1_Repeat.$2");
                        logArgs["m.NewName"] = m.NewName;
                    } while (true);
                    FileInfo newFile = new FileInfo(newFilePath);
                    File.Move(temporaryNamePath, newFile.FullName);
                    param.Print.WriteLine($"重命名: {ToShowFileName(m.Source, param.RootDire)} => {m.NewName}");
                }
                catch (Exception ex)
                {
                    log.Error("临时文件相关重命名", ex, logArgs);
                    continue;
                }
            }
            return 0;
        }
        private string CalcTemporaryName(FileInfo info, int index)
        {
            string name = $".t.n.{index}";
            do
            {
                string path = Path.Combine(info.Directory.FullName, $"{name}{info.Extension}");
                if (!File.Exists(path))
                {
                    return path;
                }
                name += ".c";
            } while (true);
        }

        private int MoveFilePosition(ParamRenameWhole param)
        {
            do
            {
                param.Print.WriteLine($"\n请输入表达式, 指定您要移动的项与位置:");
                param.Print.WriteLine($"说明: 如输入 'quit' 则直接跳过移动操作");
                param.Print.WriteLine($"示例文件队列如: 1,2,3,4,5,6,7,8,9.10");
                param.Print.WriteLine($"表达式: '7>2' 表示将第7位的文件项移动到第2位, 结果如下: 1,7,2,3,4,5,6,8,9,10");
                param.Print.WriteLine($"表达式: '7..9>2' 移动第7位到第9位文件项到第2位, 结果如下: 1,7,8,9,2,3,4,5,6,10");
                param.Print.WriteLine($"表达式: '5,7,9>2' , 移动第5,7,9项到第2项, 结果如下: 1,5,7,9,2,3,4,6,8,10");
                param.Print.WriteLine($"表达式: '2,6>4' , 移动第2,6项到第4项, 结果如下: 1,3,4,2,6,5,7,8,9,10");
                param.Print.Write($"请输入: ");
                string expression = Console.ReadLine()?.Trim()?.ToLower();
                if (string.IsNullOrEmpty(expression))
                {
                    param.Print.WriteLine("请您输入有效的表达式", EPrintColor.Red);
                    continue;
                }
                if (expression == "quit")
                    return OnExecute(param);
                var analysisResult = AnalysisMoveArrayExpression(expression);
                if (!analysisResult.IsSuccess)
                {
                    param.Print.WriteLine(analysisResult.ErrorMsg, EPrintColor.Red);
                    continue;
                }
                param.NeedHandleFileInventory = MoveArray(param.NeedHandleFileInventory,
                    analysisResult.TargetIndex, analysisResult.NeedOperationItemPositionIndex);
                return OnExecute(param);
            } while (true);
        }

        /// <summary>
        /// 解析移动数组项表达式结果
        /// </summary>
        public struct AnalysisMoveArrayExpressionResult
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            public bool IsSuccess { get; set; }
            /// <summary>
            /// 错误消息
            /// </summary>
            public string ErrorMsg { get; set; }
            /// <summary>
            /// 移动目标位置, 从0开始
            /// </summary>
            public uint TargetIndex { get; set; }
            /// <summary>
            /// 操作项位置标识, 从0开始
            /// </summary>
            public uint[] NeedOperationItemPositionIndex { get; set; }
        }

        /// <summary>
        /// 解析移动数组项表达式, 表达式中数字从1开始
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns>解析结果</returns>
        public AnalysisMoveArrayExpressionResult AnalysisMoveArrayExpression(string expression)
        {
            var result = new AnalysisMoveArrayExpressionResult() { IsSuccess = false };
            expression = Regex.Replace(expression, @"([^0-9\.>,]+)", "");
            Regex targetRegex = new Regex(@"([0-9\.>,]+)>([0-9]+)");
            Match targetMatch = targetRegex.Match(expression);
            if (!targetMatch.Success)
            {
                result.ErrorMsg = $"您输入的表达式无法识别: {expression}, 正则: {targetRegex}";
                return result;
            }
            // 获取目标位置
            string targetStrValue = targetMatch.Groups[2].Value;
            if (!uint.TryParse(targetStrValue, out uint targetIndex))
            {
                result.ErrorMsg = $"无法识别目标项标识: {targetStrValue}";
                return result;
            }
            targetIndex -= 1;
            string[] itemValues = targetMatch.Groups[1].Value.Split(new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries);
            HashSet<uint> itemIndexs = new HashSet<uint>();
            Regex itemValueRegex = new Regex(@"^([0-9]+)\.\.([0-9]+)$");
            for (int i = 0; i < itemValues.Length; i++)
            {
                string itemValue = itemValues[i];
                Match itemValueMatch = itemValueRegex.Match(itemValue);
                if (!itemValueMatch.Success)
                {
                    if (uint.TryParse(itemValue, out uint indexvalue))
                    {
                        itemIndexs.Add(indexvalue - 1);
                    }
                    continue;
                }

                if (!uint.TryParse(itemValueMatch.Groups[1].Value, out uint startIndex))
                    continue;
                if (!uint.TryParse(itemValueMatch.Groups[2].Value, out uint endIndex))
                    continue;
                uint min = Math.Min(startIndex, endIndex);
                uint max = Math.Max(startIndex, endIndex);
                for (uint j = min; j <= max; j++)
                    itemIndexs.Add(j - 1);
            }
            if (itemIndexs.Count() <= 0)
            {
                result.ErrorMsg = $"无法识别来源项标识, 计算结果为空: {targetMatch.Groups[1].Value}";
                return result;
            }
            result.IsSuccess = true;
            result.TargetIndex = targetIndex;
            result.NeedOperationItemPositionIndex = itemIndexs.ToArray();
            return result;
        }

        /// <summary>
        /// 移动数组各项的位置
        /// </summary>
        /// <typeparam name="T">数组数据类型</typeparam>
        /// <param name="list">数组队列</param>
        /// <param name="targetIndex">移动到的目标位置</param>
        /// <param name="positionIndexs">需要移动的项位置标识</param>
        /// <returns></returns>
        public T[] MoveArray<T>(T[] list, uint targetIndex, uint[] positionIndexs)
        {
            positionIndexs = positionIndexs.OrderBy(b => b).ToArray();
            T[] rlist = new T[list.Length];
            for (int i = 0; i < positionIndexs.Length; i++)
            {
                rlist[targetIndex + i] = list[positionIndexs[i]];
            }
            int i_list = 0;
            int i_rlist = 0;
            int i_position = 0;
            while (i_list < list.Length && i_rlist < rlist.Length)
            {
                if (targetIndex <= i_rlist && i_rlist <= (targetIndex + positionIndexs.Length - 1))
                {
                    i_rlist++;
                    continue;
                }
                if (i_position < positionIndexs.Length && i_list == positionIndexs[i_position])
                {
                    i_list++;
                    i_position++;
                    continue;
                }
                rlist[i_rlist] = list[i_list];
                i_rlist++;
                i_list++;
            }
            return rlist;
        }

        private int DeleteFile(ParamRenameWhole param)
        {
            do
            {
                param.Print.WriteLine($"\n请输入您要删除的项位置序号:");
                param.Print.WriteLine($"如输入 'quit' 则直接跳过删除操作");
                param.Print.Write($"请输入: ");
                string input_index = Console.ReadLine()?.Trim()?.ToLower();
                if (input_index == "quit")
                    return OnExecute(param);
                if (string.IsNullOrEmpty(input_index) || !uint.TryParse(input_index, out uint index))
                {
                    param.Print.WriteLine($"无法识别您输入的内容: {input_index}");
                    continue;
                }
                var list = new List<FileInfo>(param.NeedHandleFileInventory);
                list.RemoveAt((int)index);
                param.NeedHandleFileInventory = list.ToArray();
            } while (true);
        }
    }
}
