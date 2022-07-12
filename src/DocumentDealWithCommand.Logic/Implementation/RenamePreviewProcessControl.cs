using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 预览的流程控制
    /// </summary>
    public class RenamePreviewProcessControl
    {
        private readonly ILog log;
        private readonly IPrintColor print;
        private readonly IHandleRenameData handle;

        /// <summary>
        /// 实例化预览的流程控制
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="print">打印输出接口</param>
        /// <param name="handle">处理重命名规则接口</param>
        public RenamePreviewProcessControl(ILog log, IPrintColor print, IHandleRenameData handle)
        {
            this.log = log;
            this.print = print;
            this.handle = handle;
        }

        /// <summary>
        /// 执行预览处理结果
        /// </summary>
        /// <param name="datas">数据源队列</param>
        /// <returns>处理结果</returns>
        public IList<HandleRenameResult> OnExecutePreview(IList<FileInfo> datas)
        {
            datas = new List<FileInfo>(datas);
            do
            {
                IList<HandleRenameResult> rlist = handle.ToReanmeResult(datas);
                print.WriteLine("\n预览:");
                for (int i = 0; i < rlist.Count; i++)
                {
                    var item = rlist[i];
                    print.WriteLine($"[{i + 1}]: {handle.ToPrint(item.Source)} => {item.Result}");
                }

                do
                {
                    print.WriteLine($"\n请按提示输入 [ ] 内容标识用于后续操作:");
                    print.WriteLine($"[0]: 开始重命名, 无需做额外操作");
                    print.WriteLine($"[1]: 在队列调整文件的位置");
                    print.WriteLine($"[2]: 在队列中删除目标文件");
                    print.WriteLine($"[quit]: 终止重命名操作");
                    print.Write($"请输入: ");
                    string input = Console.ReadLine();
                    input = input?.Trim()?.ToLower();
                    if ("quit" == input)
                    {
                        print.WriteLine("您选择了终止操作, 已退出操作");
                        return null;
                    }
                    if ("0" == input)
                        return rlist;

                    if ("1" == input)
                    {
                        datas = MoveFilePosition(datas);
                        break;
                    }
                    if ("2" == input)
                    {
                        datas = DeleteFile(datas);
                        break;
                    }
                    print.Write($"未识别到您的输入内容! {input}", EPrintColor.Red);
                } while (true);
            } while (true);
        }

        private IList<FileInfo> MoveFilePosition(IList<FileInfo> datas)
        {
            do
            {
                print.WriteLine($"\n请输入表达式, 指定您要移动的项与位置:");
                print.WriteLine($"说明: 如输入 'quit' 则直接跳过移动操作");

                print.Write($"请输入: ");
                string expression = Console.ReadLine();
                expression = expression?.Trim()?.ToLower();
                if (string.IsNullOrEmpty(expression))
                {
                    print.WriteLine("\n请您输入有效的表达式", EPrintColor.Red);
                    print.WriteLine("示例文件队列如:                                           1,2,3,4,5,6,7,8,9.10");
                    print.WriteLine("表达式: '7>2'      表示将第7位的文件项移动到第2位  结果如下: 1,7,2,3,4,5,6,8,9,10");
                    print.WriteLine("表达式: '7..9>2'   移动第7位到第9位文件项到第2位   结果如下: 1,7,8,9,2,3,4,5,6,10");
                    print.WriteLine("表达式: '5,7,9>2'  移动第5,7,9项到第2项           结果如下: 1,5,7,9,2,3,4,6,8,10");
                    print.WriteLine("表达式: '2,6>4'    移动第2,6项到第4项             结果如下: 1,3,4,2,6,5,7,8,9,10");
                    continue;
                }
                if (expression == "quit")
                    return datas;

                var analysisResult = AnalysisMoveArrayExpression(expression);
                if (!analysisResult.IsSuccess)
                {
                    print.WriteLine(analysisResult.ErrorMsg, EPrintColor.Red);
                    continue;
                }
                datas = MoveUserIndexItems(datas, analysisResult);
                return datas;
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
                        itemIndexs.Add(indexvalue);
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
                    itemIndexs.Add(j);
            }
            if (itemIndexs.Count() <= 0)
            {
                result.ErrorMsg = $"无法识别来源项标识, 计算结果为空: {targetMatch.Groups[1].Value}";
                return result;
            }
            result.IsSuccess = true;
            result.TargetIndex = targetIndex;
            result.NeedOperationItemPositionIndex = itemIndexs
                .OrderBy(b => b)
                .ToArray();
            return result;
        }

        /// <summary>
        /// 移动用户指定标识项
        /// </summary>
        /// <typeparam name="T">数据队列数据类型</typeparam>
        /// <param name="list">数据队列</param>
        /// <param name="r">解析用户指定标识结果</param>
        /// <returns>移动结果项</returns>
        public IList<T> MoveUserIndexItems<T>(IList<T> list, AnalysisMoveArrayExpressionResult r)
        {
            r.TargetIndex = ArrayDataExtend.UserInputToProgramTargetIndex(r.TargetIndex, list.Count());
            r.NeedOperationItemPositionIndex = ArrayDataExtend.UserInputToProgramTargetIndexs(r.NeedOperationItemPositionIndex, list.Count());
            return list.MoveArray(r.TargetIndex, r.NeedOperationItemPositionIndex);
        }

        private IList<FileInfo> DeleteFile(IList<FileInfo> datas)
        {
            do
            {
                print.WriteLine($"\n请输入您要删除的项位置序号:");
                print.WriteLine($"如输入 'quit' 则直接跳过删除操作");
                print.Write($"请输入: ");
                string input_index = Console.ReadLine();
                input_index = input_index?.Trim()?.ToLower();
                if (input_index == "quit")
                    return datas;

                if (string.IsNullOrEmpty(input_index) || !uint.TryParse(input_index, out uint index))
                {
                    print.WriteLine($"无法识别您输入的内容: {input_index}");
                    continue;
                }
                datas = RemoveUserIndexItem(datas, index);
            } while (true);
        }

        /// <summary>
        /// 用户指定删除目标项
        /// </summary>
        /// <param name="datas">数据列表</param>
        /// <param name="index">用户输入目标位置标识(从1开始)</param>
        /// <returns>返回删除后的列表</returns>
        public IList<T> RemoveUserIndexItem<T>(IList<T> datas, uint index)
        {
            if (datas.IsReadOnly)
                datas = new List<T>(datas);
            index = ArrayDataExtend.UserInputToProgramTargetIndex(index, datas.Count);
            datas.RemoveAt((int)index);
            return datas;
        }
    }
}
