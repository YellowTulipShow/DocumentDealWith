using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using DocumentDealWithCommand.Logic.Models;

using CodingSupportLibrary;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 编码修改
    /// </summary>
    public class Main_Content_Encoding : AbsMain, IMain<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public Main_Content_Encoding(ILog log, IPrintColor print) : base(log, print)
        {
            UseExtend.SupportCodePages();
        }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Content_Encoding commandParameters)
        {
            var rinventory = commandParameters.NeedHandleFileInventory;
            foreach (var item in rinventory)
            {
                ConvertFileEncoding(item, commandParameters.Target, log, print);
            }
            return 0;
        }

        /// <summary>
        /// 转换文件编码
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="target">目标编码</param>
        /// <param name="log">日志接口</param>
        /// <param name="print">打印输出接口, 可为空</param>
        /// <exception cref="Exception">执行过程中发生的异常</exception>
        public static void ConvertFileEncoding(FileInfo file, ESupportEncoding target, ILog log, IPrintColor print = null)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["file"] = file.FullName;
            logArgs["target"] = target.ToString();
            try
            {
                var response = file.GetJudgeEncodingResponse();
                if (response.Encoding == null)
                {
                    log.Error($"文件: {file.FullName} 无法识别文件编码格式!");
                    return;
                }
                logArgs["response.Encoding"] = response.Encoding.ToString();
                logArgs["response.IsReadFileALLContent"] = response.IsReadFileALLContent;

                // 拿到文件相关数据
                Encoding fileEncoding = response.Encoding.ToEncoding();
                logArgs["fileEncoding"] = fileEncoding.EncodingName;
                // 拿到内容
                byte[] fileBytes = response.IsReadFileALLContent ?
                    response.ContentBytes :
                    File.ReadAllBytes(file.FullName);
                // 如果文件内容带有头部信息则删掉
                if (fileEncoding.Preamble.Length > 0)
                    fileBytes = fileBytes.Skip(fileEncoding.Preamble.Length).ToArray();
                // 准备转换目标信息
                Encoding targetEncoding = target.ToEncoding();
                logArgs["targetEncoding"] = targetEncoding.EncodingName;
                // 转换完成写入到文件中
                List<byte> rlist = new List<byte>();
                if (targetEncoding.Preamble.Length > 0)
                    rlist.AddRange(targetEncoding.Preamble.ToArray());
                byte[] targetBytes = Encoding.Convert(fileEncoding, targetEncoding, fileBytes);
                rlist.AddRange(targetBytes);
                File.WriteAllBytes(file.FullName, rlist.ToArray());
                print?.WriteLine($"文件编码转换成功! {file.FullName} | {response.Encoding} To {target}");
            }
            catch (Exception ex)
            {
                log.Error("转换文件出错", ex, logArgs);
                throw ex;
            }
        }
    }
}