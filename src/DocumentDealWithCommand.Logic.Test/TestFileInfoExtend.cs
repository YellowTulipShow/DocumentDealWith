using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using YTS.Log;
using YTS.ConsolePrint;

using CodingSupportLibrary;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestFileInfoExtend
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestFileInfoExtend");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_ToShowFileName()
        {
            string direPath = @"D:\Work\YTS.ZRQ\DocumentDealWith\_test\";
            DirectoryInfo dire = new DirectoryInfo(direPath);
            string filePath = @"D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename\code.txt";
            FileInfo fi = new FileInfo(filePath);
            Assert.AreEqual(@"/D/Work/YTS.ZRQ/DocumentDealWith/_test/rename/code.txt", fi.ToShowFileName());
            Assert.AreEqual(@"./rename/code.txt", fi.ToShowFileName(dire));
            direPath = @"D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename";
            dire = new DirectoryInfo(direPath);
            Assert.AreEqual(@"./code.txt", fi.ToShowFileName(dire));
        }
    }
}
