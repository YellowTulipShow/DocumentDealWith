using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

using YTS.Log;

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
            Assert.AreEqual(@"~/rename/code.txt", fi.ToShowFileName(dire));
            direPath = @"D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename";
            dire = new DirectoryInfo(direPath);
            Assert.AreEqual(@"~/code.txt", fi.ToShowFileName(dire));
        }

        [TestMethod]
        public void Test_ToMFileName()
        {
            void Test(string filePath, string expected_name, string expected_extension)
            {
                MFileName m = FileInfoExtend.ToMFileName(filePath);
                Assert.AreEqual(expected_name, m.Name);
                Assert.AreEqual(expected_extension, m.Extension);
            }

            Test(@"", "", "");
            Test(@".", "", ".");
            Test(@"txt", "txt", "");
            Test(@".txt", "", ".txt");
            Test(@"t.xt", "t", ".xt");
            Test(@"tx.t", "tx", ".t");
            Test(@"txt.", "txt", "");
            Test(@".t.x.t.", ".t.x.t", ".");
            Test(@".t.x.t.tt", ".t.x.t", "tt");
            Test(@"code.txt", "code", ".txt");

            Test(@".\code.txt", "code", ".txt");
            Test(@"~\DD\code.txt", "code", ".txt");
            Test(@"\code.txt", "code", ".txt");
            Test(@"D\code.txt", "code", ".txt");
            Test(@"D\code.txt", "code", ".txt");
            Test(@"D:\code.txt", "code", ".txt");
            Test(@"D:\rename\code.txt", "code", ".txt");
            Test(@"D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename\code.txt", "code", ".txt");

            Test(@"./code.txt", "code", ".txt");
            Test(@"~/DD/code.txt", "code", ".txt");
            Test(@"/code.txt", "code", ".txt");
            Test(@"D/code.txt", "code", ".txt");
            Test(@"/D/code.txt", "code", ".txt");
            Test(@"/D/code.txt", "code", ".txt");
            Test(@"/D/code.txt", "code", ".txt");
            Test(@"/D/rename/code.txt", "code", ".txt");
            Test(@"/D/Work/YTS.ZRQ/DocumentDealWith/_test/rename/code.txt", "code", ".txt");
            Test(@"/D:/code.txt", "code", ".txt");
            Test(@"/D:/rename/code.txt", "code", ".txt");
            Test(@"/D:/Work/YTS.ZRQ/DocumentDealWith/_test/rename/code.txt", "code", ".txt");
        }
    }
}
