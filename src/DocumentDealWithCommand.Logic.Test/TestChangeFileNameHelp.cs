using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Text;

using YTS.Log;

using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestChangeFileNameHelp
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestChangeFileNameHelp");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_ChangeFileName()
        {
            Encoding editEncoding = Encoding.UTF8;
            var help = new ChangeFileNameHelp(log);
            void Test(string sourceFileName, string targetFileName)
            {
                string expectedFilePath = $"./changeFileNameDire/{sourceFileName}.txt";
                FileInfo codeFile = new FileInfo(Path.Combine(Environment.CurrentDirectory,
                    $"./changeFileNameDire/{sourceFileName}.txt"));
                if (!codeFile.Directory.Exists)
                    codeFile.Directory.Create();
                string text = expectedFilePath;
                File.WriteAllText(codeFile.FullName, text, editEncoding);
            }

            Assert.IsTrue(false);
        }
    }
}
