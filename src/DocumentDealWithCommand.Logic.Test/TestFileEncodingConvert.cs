using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Text;

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
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private struct FileArg
        {
            public string FilePath { get; set; }
            public ESupportEncoding TargetEncoding { get; set; }
        }
        [TestMethod]
        public void TestReadFileBytes()
        {
            FileArg[] fileArgs = new FileArg[]
            {
            };
            for (int i = 0; i < fileArgs.Length; i++)
            {
                FileArg fileArg = fileArgs[i];
                FileInfo file = new FileInfo(fileArg.FilePath);
                Encoding targetEncoding = fileArg.TargetEncoding.ToEncoding();
                Main_Content_Encoding.ConvertFileEncoding(file, fileArg.TargetEncoding, log, null);
                Encoding convert_after_Encoding = file.GetEncoding();

                Assert.IsNotNull(convert_after_Encoding);
                Assert.AreEqual(targetEncoding.BodyName, convert_after_Encoding.BodyName);
                Assert.AreEqual(targetEncoding.HeaderName, convert_after_Encoding.HeaderName);
                Assert.AreEqual(targetEncoding.WebName, convert_after_Encoding.WebName);
                Assert.AreEqual(targetEncoding.EncodingName, convert_after_Encoding.EncodingName);
            }
            Assert.IsTrue(true);
        }
    }
}
