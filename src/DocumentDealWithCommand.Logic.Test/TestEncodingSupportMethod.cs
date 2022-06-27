using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using YTS.Log;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestEncodingSupportMethod
    {
        private ILog log;
        private const string fileContent = @"///撒旦<summary>";
        private string[] encodingNames;

        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestEncodingSupportMethod");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            encodingNames = new string[] {
                "utf-8",
                "GBK",
                //"IBM01145",
                //"IBM285",
                //"iso-8859-2",
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void TestReadFileBytes()
        {
            UTF8Encoding genc = new UTF8Encoding(false);

            byte[] expectedDatas = genc.GetBytes(fileContent);
            string expected = genc.GetString(expectedDatas).Trim();

            foreach (string name in encodingNames)
            {
                StringBuilder str = new StringBuilder();
                Encoding encoding = Encoding.GetEncoding(name);
                str.AppendLine(string.Empty);
                str.AppendLine($"{name} BodyName: {encoding.BodyName} | HeaderName: {encoding.HeaderName} | WebName: {encoding.WebName}");
                str.AppendLine($"{name}.EncodingName: {encoding.EncodingName}");

                string codeFilePath = Path.Combine(Environment.CurrentDirectory, $"./codefiles/{name}.txt");
                FileInfo codeFile = new FileInfo(codeFilePath);
                if (!codeFile.Directory.Exists)
                {
                    codeFile.Directory.Create();
                }
                str.AppendLine($"{name} FileCodePath: {codeFile.FullName}");
                File.WriteAllText(codeFile.FullName, fileContent, encoding);

                byte[] datas = File.ReadAllBytes(codeFile.FullName);
                for (int i = 0; i < datas.Length; i++)
                {
                    str.Append($"({i}):[0x{datas[i]:X2}] ");
                }
                str.AppendLine(string.Empty);
                log.Info(str.ToString());

                byte[] rdatas = Encoding.Convert(encoding, genc, datas);
                if (name == "utf-8")
                {
                    rdatas = rdatas.Skip(3).ToArray();
                }
                string rstr = genc.GetString(rdatas).Trim();
                Assert.AreEqual(expected.Length, rstr.Length);

                var dd = Encoding.GetEncoding(datas);
            }
            Assert.IsTrue(true);
        }
    }
}
