using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var help = new ChangeFileNameHelp(log);
            Assert.IsTrue(false);
        }
    }
}
