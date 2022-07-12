using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestMain_Rename_AddOrDelete
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestMain_Rename_AddOrDelete");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private static ParamRenameAddOrDelete GetDefalutParam()
        {
            return new ParamRenameAddOrDelete()
            {
                RootDire = new DirectoryInfo("C:\\"),
                Config = null,
                ConsoleType = EConsoleType.WindowGitBash,

                IsPreview = true,

                AfterAdd = "",
                BeforeAdd = "",
                DeleteContent = "",
                IsUseExtendAdd = false,
                IsUseExtendDelete= false,
                UseExtendAddContent = "",
                UseExtendAddStartCharIndex = 0,
                UseExtendDeleteCount = 0,
                UseExtendDeleteStartCharIndex = 0,
            };
        }

        [TestMethod]
        public void Test_ToResult()
        {
            var main = new Main_Rename_AddOrDelete(log);
            ParamRenameAddOrDelete param = GetDefalutParam();
            main.SetParam(param);

            Assert.IsTrue(false);
        }
    }
}
