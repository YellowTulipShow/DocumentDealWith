using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using YTS.Log;

using CodingSupportLibrary;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestMain_Content_NewLine
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestFileNewLineSymbolConvert");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());

            // 测试开始前引用官方代码页引用, 增加支持中文GBK
            UseExtend.SupportCodePages();
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private struct FileArg
        {
            public string SourceText { get; set; }
            public ENewLineType TargetType { get; set; }
            public string ResultText { get; set; }
        }
        private FileArg[] PrepareTestData()
        {
            IList<FileArg> rlist = new List<FileArg>();
            const string str = @"JSON在线 | JSON解析格式化—SO JSON在线工具";
            string[] symbols = new string[] { $"\r\n", $"\n\r", $"\r", $"\n" };
            const string replace_center_str = "!!!~####~!!!";
            for (int index_str = 0; index_str < str.Length; index_str++)
            {
                foreach (string symbol in symbols)
                {
                    foreach (ENewLineType type in Enum.GetValues(typeof(ENewLineType)))
                    {
                        string SourceText = str.Insert(index_str, symbol);
                        string ResultText = SourceText;
                        string typeChar = type == ENewLineType.LF ? "\n" :
                            type == ENewLineType.CR ? "\r" :
                            type == ENewLineType.CRLF ? "\r\n" : "";
                        foreach (string symbol2 in symbols)
                        {
                            ResultText = ResultText.Replace(symbol2, replace_center_str);
                        }
                        ResultText = ResultText.Replace(replace_center_str, typeChar);
                        rlist.Add(new FileArg()
                        {
                            SourceText = SourceText,
                            TargetType = type,
                            ResultText = ResultText,
                        });
                    }
                }
            }
            return rlist.ToArray();
        }
        [TestMethod]
        public void TestConvertFileNewLineSymbol()
        {
            FileArg[] fileArgs = new FileArg[]
            {
            };
            if (fileArgs == null || fileArgs.Length <= 0)
                fileArgs = PrepareTestData();
            var logArgs = log.CreateArgDictionary();
            FileInfo file = null;
            try
            {
                Encoding GBK = Encoding.GetEncoding("GBK");

                for (int i = 0; i < fileArgs.Length; i++)
                {
                    FileArg fileArg = fileArgs[i];
                    logArgs["fileArg.SourceText"] = fileArg.SourceText;
                    logArgs["fileArg.TargetType"] = fileArg.TargetType.ToString();
                    logArgs["fileArg.ResultText"] = fileArg.ResultText;
                    logArgs["fileArg.ResultBytes"] = GBK.GetBytes(fileArg.ResultText).ToX2FormatString();

                    file = new FileInfo(Path.Combine(Environment.CurrentDirectory,
                        $"./codefiles/TestConvertFileNewLineSymbol/{fileArg.SourceText.Length}_{fileArg.TargetType}_{fileArg.ResultText.Length}_{i}.txt"));
                    if (!file.Directory.Exists)
                        file.Directory.Create();
                    logArgs["filePath"] = file.FullName;
                    File.WriteAllText(file.FullName, fileArg.SourceText, GBK);
                    logArgs["file.ConvertBefore.Text"] = file.ReadFileBytesToX2FormatString();
                    // 转换文件换行符号
                    Main_Content_NewLine.ConvertFileNewLineSymbol(file, fileArg.TargetType, log, null);
                    logArgs["file.ConvertAfter.Text"] = file.ReadFileBytesToX2FormatString();
                    // 获取转换后文件内容
                    string convert_after_Content = File.ReadAllText(file.FullName, GBK);
                    // 判断内容是否还是一样的
                    Assert.AreEqual(fileArg.ResultText, convert_after_Content);
                }
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                log.Error("测试(TestConvertFileNewLineSymbol)出错", ex, logArgs);
                log.Info(file.GetWriteLogFileContentBytesContent());
                throw ex;
            }
        }
    }
}
