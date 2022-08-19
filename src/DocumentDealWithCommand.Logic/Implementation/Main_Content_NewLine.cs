using System;
using System.IO;
using System.Collections.Generic;

using YTS.Log;
using YTS.ConsolePrint;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 换行符类型更改
    /// </summary>
    public class Main_Content_NewLine : AbsMain, IExecute<CommandParameters_Content_NewLine>
    {
        private const byte LF = (byte)ENewLineType.LF;
        private const byte CR = (byte)ENewLineType.CR;

        /// <inheritdoc/>
        public Main_Content_NewLine(ILog log) : base(log) { }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Content_NewLine commandParameters)
        {
            var rinventory = commandParameters.NeedHandleFileInventory;
            foreach (var item in rinventory)
            {
                ConvertFileNewLineSymbol(item, commandParameters.Type, log);
            }
            return 0;
        }

        /// <summary>
        /// 转换文件换行标识
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="newLineSymbol">目标换行标识</param>
        /// <param name="log">日志接口</param>
        /// <param name="print">打印输出接口, 可为空</param>
        /// <exception cref="Exception">执行过程中发生的异常</exception>
        public static void ConvertFileNewLineSymbol(FileInfo file, ENewLineType newLineSymbol, ILog log, IPrintColor print = null)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["file"] = file.FullName;
            logArgs["newLineSymbol"] = newLineSymbol.ToString();
            try
            {
                // 拿到内容
                byte[] fileBytes = File.ReadAllBytes(file.FullName);
                List<byte> rlist = new List<byte>(fileBytes);

                // 拿到目标
                byte[] targetBytes = ToTypeBytes(newLineSymbol);

                int need_install_index = -1;
                int index = 0;
                while (index < rlist.Count)
                {
                    byte b = rlist[index];
                    if (b != LF && b != CR)
                    {
                        if (need_install_index != -1)
                        {
                            for (int targetBytesIndex = targetBytes.Length - 1; targetBytesIndex >= 0; targetBytesIndex--)
                            {
                                rlist.Insert(need_install_index, targetBytes[targetBytesIndex]);
                            }
                            need_install_index = -1;
                            index += targetBytes.Length + 1;
                            continue;
                        }
                        index++;
                        continue;
                    }
                    // 判断是否记录的删除最开始的位置
                    if (need_install_index == -1)
                        need_install_index = index;
                    // 删除这个字节
                    rlist.RemoveAt(index);
                }
                File.WriteAllBytes(file.FullName, rlist.ToArray());
                print?.WriteLine($"文件换行符转换成功! {file.FullName} => {newLineSymbol}");
            }
            catch (Exception ex)
            {
                log.Error("转换文件出错", ex, logArgs);
                throw ex;
            }
        }

        private static byte[] ToTypeBytes(ENewLineType newLineSymbol)
        {
            return newLineSymbol switch
            {
                ENewLineType.LF => new byte[] { LF },
                ENewLineType.CR => new byte[] { CR },
                ENewLineType.CRLF => new byte[] { CR, LF },
                _ => throw new ArgumentOutOfRangeException(nameof(newLineSymbol), $"换行符, 无法解析: {newLineSymbol}"),
            };
        }
    }
}
