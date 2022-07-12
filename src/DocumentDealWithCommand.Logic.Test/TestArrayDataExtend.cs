using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text;

using YTS.Log;

using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestArrayDataExtend
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestArrayDataExtend");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_UserInputToProgramTargetIndex()
        {
            void Test(int totalListCount, uint userInputIndex, uint programTargetIndex)
            {
                uint r = ArrayDataExtend.UserInputToProgramTargetIndex(userInputIndex, totalListCount);
                Assert.AreEqual(r, programTargetIndex);
            }
            Test(8, 0, 0);
            Test(8, 1, 0);
            Test(8, 2, 1);
            Test(8, 3, 2);
            Test(8, 4, 3);
            Test(8, 5, 4);
            Test(8, 6, 5);
            Test(8, 7, 6);
            Test(8, 8, 7);
            Test(8, 9, 7);
            Test(8, 10, 7);
        }
    }
}
