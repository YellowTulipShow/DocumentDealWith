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
                UseExtendAddContent = "",
                UseExtendAddStartCharIndex = 0,

                IsUseExtendDelete = false,
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

            void Test(string expected, string filePath)
            {
                MFileName m = FileInfoExtend.ToMFileName(filePath);
                string result = main.ARFileName(m, param);
                Assert.AreEqual(expected, result);
            }

            param.AfterAdd = "DDD";
            Test("DDDtest_1.jpg", "test_1.jpg");
            param.BeforeAdd = "DDD";
            Test("DDDtest_1DDD.jpg", "test_1.jpg");
            param.DeleteContent = "_1";
            Test("DDDtestDDD.jpg", "test_1.jpg");

            param.AfterAdd = "";
            param.BeforeAdd = "";
            param.DeleteContent = "";
            param.IsUseExtendAdd = true;
            param.UseExtendAddContent = "EEE";
            param.UseExtendAddStartCharIndex = 0;
            Test("EEEtest_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 1;
            Test("EEEtest_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 2;
            Test("tEEEest_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 3;
            Test("teEEEst_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 4;
            Test("teEEEst_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 5;
            Test("tesEEEt_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 6;
            Test("testEEE_1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 7;
            Test("test_EEE1.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 8;
            Test("test_1EEE.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 9;
            Test("test_1EEE.jpg", "test_1.jpg");
            param.UseExtendAddStartCharIndex = 10;
            Test("test_1EEE.jpg", "test_1.jpg");
            param.IsUseExtendAdd = false;

            param.IsUseExtendDelete = true;
            param.UseExtendDeleteCount = 1;
            param.UseExtendDeleteStartCharIndex = 0;
            Test("est_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 1;
            Test("est_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 2;
            Test("tst_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 3;
            Test("tet_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 4;
            Test("tes_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 5;
            Test("test1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 6;
            Test("test_.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 7;
            Test("test_.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 8;
            Test("test_.jpg", "test_1.jpg");

            param.UseExtendDeleteCount = 0;
            param.UseExtendDeleteStartCharIndex = 0;
            Test("test_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 1;
            Test("test_1.jpg", "test_1.jpg");

            param.UseExtendDeleteCount = 7;
            param.UseExtendDeleteStartCharIndex = 0;
            Test(".jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 1;
            Test(".jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 9;
            Test(".jpg", "test_1.jpg");

            param.UseExtendDeleteCount = 4;
            param.UseExtendDeleteStartCharIndex = 0;
            Test("_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 1;
            Test("t1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 2;
            Test("te.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 3;
            Test("tes.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 4;
            Test("test.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 5;
            Test("test_.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 6;
            Test("test_1.jpg", "test_1.jpg");
            param.UseExtendDeleteStartCharIndex = 7;
            Test("test_1.jpg", "test_1.jpg");

            Assert.IsTrue(false);
        }
    }
}
