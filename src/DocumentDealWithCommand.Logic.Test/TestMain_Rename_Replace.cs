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
    public class TestMain_Rename_Replace
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestMain_Rename_Replace");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        private static ParamRenameReplace GetDefalutParam()
        {
            return new ParamRenameReplace()
            {
                RootDire = new DirectoryInfo("C:\\"),
                Config = null,
                ConsoleType = EConsoleType.WindowGitBash,

                IsPreview = true,
                Pattern = "",
                Replacement = "",
            };
        }

        [TestMethod]
        public void Test_RegexReplaceFileName()
        {
            var main = new Main_Rename_Replace(log);
            ParamRenameReplace param = GetDefalutParam();
            main.SetParam(param);

            param.Pattern = @"^test_(\w+)\.(\w+)";
            param.Replacement = "$2_$1_$2.$2";

            Assert.AreEqual("test_a.jpg", main.RegexReplaceFileName("jpg_a_jpg.jpg", param));
            Assert.AreEqual("test_b.jpg", main.RegexReplaceFileName("jpg_b_jpg.jpg", param));
            Assert.AreEqual("test_cdd.jpg", main.RegexReplaceFileName("jpg_cdd_jpg.jpg", param));

            Assert.AreEqual("test_a.png", main.RegexReplaceFileName("png_a_png.png", param));
            Assert.AreEqual("test_b.png", main.RegexReplaceFileName("png_b_png.png", param));
            Assert.AreEqual("test_cdd.png", main.RegexReplaceFileName("png_cdd_png.png", param));
        }
    }
}
