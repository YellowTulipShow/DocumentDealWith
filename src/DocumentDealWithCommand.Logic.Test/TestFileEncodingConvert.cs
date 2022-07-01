using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using YTS.Log;

using CodingSupportLibrary;

using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestFileEncodingConvert
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestFileEncodingConvert");
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
            public string Text { get; set; }
            public ESupportEncoding WriteEncoding { get; set; }
            public ESupportEncoding TargetEncoding { get; set; }
        }
        private FileArg[] PrepareTestData()
        {
            IList<FileArg> rlist = new List<FileArg>();
            void addTestDatas(string[] texts, ESupportEncoding[] encodings)
            {
                foreach (string txt in texts)
                {
                    foreach (ESupportEncoding writeEncoding in encodings)
                    {
                        foreach (ESupportEncoding targetEncoding in encodings)
                        {
                            if (writeEncoding == targetEncoding)
                                continue;
                            rlist.Add(new FileArg()
                            {
                                Text = txt,
                                WriteEncoding = writeEncoding,
                                TargetEncoding = targetEncoding,
                            });
                        }
                    }
                }
            }

            // 英文
            string[] text_en = new string[] {
                @"<>33,32,2.52.",
                @"/// sLiellekg <summary>",
                @"/// lsdijfliajlsdjlfajg",
            };
            ESupportEncoding[] encodings_en = new ESupportEncoding[] {
                ESupportEncoding.UTF32_LittleEndian,
                ESupportEncoding.UTF16_BigEndian,
                ESupportEncoding.UTF16_LittleEndian,
                ESupportEncoding.UTF8,
                ESupportEncoding.UTF8_NoBOM,
                ESupportEncoding.GBK,
                ESupportEncoding.ASCII,
            };
            addTestDatas(text_en, encodings_en);

            // 中文
            string[] text_zh = new string[] {
                @"///撒旦<summary>",
                @"JSON在线 | JSON解析格式化—SO JSON在线工具",
                @"善良的看看/*-+",

                @"<>33,32,2.52.",
                @"u3li23k(&@L@)*!??SOF_W@_L@|>:OL~~!``;fdi12+d-3+lsieig"
            };
            ESupportEncoding[] encodings_zh = new ESupportEncoding[] {
                ESupportEncoding.UTF32_LittleEndian,
                ESupportEncoding.UTF16_BigEndian,
                ESupportEncoding.UTF16_LittleEndian,
                ESupportEncoding.UTF8,
                ESupportEncoding.UTF8_NoBOM,
                ESupportEncoding.GBK,
            };
            addTestDatas(text_zh, encodings_zh);
            return rlist.ToArray();
        }
        [TestMethod]
        public void TestConvertFileEncoding()
        {
            FileArg[] fileArgs = new FileArg[]
            {
            };
            if (fileArgs == null || fileArgs.Length <= 0)
                fileArgs = PrepareTestData();
            var logArgs = log.CreateArgDictionary();
            try
            {
                for (int i = 0; i < fileArgs.Length; i++)
                {
                    FileArg fileArg = fileArgs[i];
                    logArgs["fileArg.Text"] = fileArg.Text;
                    logArgs["fileArg.WriteEncoding"] = fileArg.WriteEncoding.ToString();
                    logArgs["fileArg.TargetEncoding"] = fileArg.TargetEncoding.ToString();
                    FileInfo file = new FileInfo(Path.Combine(Environment.CurrentDirectory,
                        $"./codefiles/TestFileEncodingConvert_{fileArg.WriteEncoding}_TO_{fileArg.TargetEncoding}_{i}.txt"));
                    if (!file.Directory.Exists)
                        file.Directory.Create();
                    logArgs["filePath"] = file.FullName;
                    Encoding fileEncoding = fileArg.WriteEncoding.ToEncoding();
                    File.WriteAllText(file.FullName, fileArg.Text, fileEncoding);
                    // 获取转换目标编码
                    Encoding targetEncoding = fileArg.TargetEncoding.ToEncoding();
                    // 转换文件编码
                    Main_Content_Encoding.ConvertFileEncoding(file, fileArg.TargetEncoding, log, null);
                    // 获取转换后文件编码
                    Encoding convert_after_Encoding = file.GetEncoding();
                    Assert.IsNotNull(convert_after_Encoding);
                    // 任何编码都支持最基本的ASCII编码, 如果判断文本内容都是单字节ASCII编码, 使用任何编码读取结果都是一样的
                    if (convert_after_Encoding.Equals(Encoding.ASCII))
                        convert_after_Encoding = targetEncoding;
                    // 比较目标编码和转换后编码是否相等
                    Assert.AreEqual(targetEncoding.BodyName, convert_after_Encoding.BodyName);
                    Assert.AreEqual(targetEncoding.HeaderName, convert_after_Encoding.HeaderName);
                    Assert.AreEqual(targetEncoding.WebName, convert_after_Encoding.WebName);
                    Assert.AreEqual(targetEncoding.EncodingName, convert_after_Encoding.EncodingName);
                    // 获取转换后文件内容
                    string convert_after_Content = File.ReadAllText(file.FullName, convert_after_Encoding);
                    // 判断内容是否还是一样的
                    Assert.AreEqual(fileArg.Text, convert_after_Content);
                }
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                log.Error("测试(TestConvertFileEncoding)出错", ex, logArgs);
                throw ex;
            }
        }

        [TestMethod]
        public void TestEncodingPreambleHaveValue()
        {
            Encoding en = ESupportEncoding.ASCII.ToEncoding();
            Assert.IsTrue(en.Preamble.Length == 0);

            en = ESupportEncoding.UTF32_LittleEndian.ToEncoding();
            Assert.IsTrue(en.Preamble.Length > 0);

            en = ESupportEncoding.UTF16_BigEndian.ToEncoding();
            Assert.IsTrue(en.Preamble.Length > 0);

            en = ESupportEncoding.UTF16_LittleEndian.ToEncoding();
            Assert.IsTrue(en.Preamble.Length > 0);

            en = ESupportEncoding.UTF8.ToEncoding();
            Assert.IsTrue(en.Preamble.Length > 0);

            en = ESupportEncoding.UTF8_NoBOM.ToEncoding();
            Assert.IsTrue(en.Preamble.Length == 0);

            en = ESupportEncoding.GBK.ToEncoding();
            Assert.IsTrue(en.Preamble.Length == 0);

            en = Encoding.UTF7;
            Assert.IsTrue(en.Preamble.Length == 0);
        }
    }
}
