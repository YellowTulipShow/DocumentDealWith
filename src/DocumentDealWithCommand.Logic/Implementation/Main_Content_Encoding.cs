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
            log.Info($"转换编码, 参数 - Encoding: {commandParameters.Target}");

            var rinventory = commandParameters.NeedHandleFileInventory;
            foreach (var item in rinventory)
            {
                print.WriteLine($"文件路径: {item.FullName}");
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
                Encoding fileEncoding = response.Encoding.ToEncoding();
                logArgs["fileEncoding"] = fileEncoding.EncodingName;
                Encoding targetEncoding = target.ToEncoding();
                logArgs["targetEncoding"] = targetEncoding.EncodingName;
                byte[] fileBytes = response.IsReadFileALLContent ?
                    response.ContentBytes :
                    File.ReadAllBytes(file.FullName);
                byte[] targetBytes = Encoding.Convert(fileEncoding, targetEncoding, fileBytes);
                File.WriteAllBytes(file.FullName, targetBytes);
                print?.WriteLine($"文件编码转换成功: ")
            }
            catch (Exception ex)
            {
                log.Error("转换文件出错", ex, logArgs);
                throw ex;
            }
        }
    }
}