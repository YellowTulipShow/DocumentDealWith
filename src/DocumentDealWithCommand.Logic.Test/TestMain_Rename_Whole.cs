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
    public class TestMain_Rename_Whole
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestMain_Rename_Whole");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void TestToNewName()
        {
            var main = new Main_Rename_Whole(log);
            ParamRenameWhole param = new ParamRenameWhole()
            {
                RootDire = new DirectoryInfo("C:\\"),
                Config = null,
                ConsoleType = EConsoleType.WindowGitBash,

                IsPreview = true,
                NamingRules = "*",
                StartedOnIndex = 1,
                Increment = 1,
                Digit = 1,
                IsInsufficientSupplementBit = true,
                IsUseLetter = false,
                UseLetterFormat = ERenameLetterFormat.Lower,
                IsChangeExtension = false,
                ChangeExtensionValue = null,
                IsAutomaticallyResolveRenameConflicts = true,
            };
            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", param, 0));
            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", param, 1));
            Assert.AreEqual("test.jpg", main.ToNewName("test.jpg", param, 2));

            param.NamingRules = "*_#";
            Assert.AreEqual("test_1.jpg", main.ToNewName("test.jpg", param, 0));
            Assert.AreEqual("test_2.jpg", main.ToNewName("test.jpg", param, 1));
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", param, 2));
            Assert.AreEqual("test_23.jpg", main.ToNewName("test.jpg", param, 22));
            Assert.AreEqual("test_416.jpg", main.ToNewName("test.jpg", param, 415));

            param.StartedOnIndex = 3;
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", param, 0));
            Assert.AreEqual("test_4.jpg", main.ToNewName("test.jpg", param, 1));
            Assert.AreEqual("test_5.jpg", main.ToNewName("test.jpg", param, 2));
            Assert.AreEqual("test_25.jpg", main.ToNewName("test.jpg", param, 22));
            Assert.AreEqual("test_418.jpg", main.ToNewName("test.jpg", param, 415));

            param.StartedOnIndex = 1;
            param.Increment = 2;
            Assert.AreEqual("test_1.jpg", main.ToNewName("test.jpg", param, 0));
            Assert.AreEqual("test_3.jpg", main.ToNewName("test.jpg", param, 1));
            Assert.AreEqual("test_5.jpg", main.ToNewName("test.jpg", param, 2));
            Assert.AreEqual("test_7.jpg", main.ToNewName("test.jpg", param, 3));
            Assert.AreEqual("test_9.jpg", main.ToNewName("test.jpg", param, 4));
            Assert.AreEqual("test_11.jpg", main.ToNewName("test.jpg", param, 5));
            Assert.AreEqual("test_13.jpg", main.ToNewName("test.jpg", param, 6));
            Assert.AreEqual("test_15.jpg", main.ToNewName("test.jpg", param, 7));
            Assert.AreEqual("test_17.jpg", main.ToNewName("test.jpg", param, 8));
            Assert.AreEqual("test_19.jpg", main.ToNewName("test.jpg", param, 9));
            Assert.AreEqual("test_21.jpg", main.ToNewName("test.jpg", param, 10));
            Assert.AreEqual("test_23.jpg", main.ToNewName("test.jpg", param, 11));
        }
    }
}
