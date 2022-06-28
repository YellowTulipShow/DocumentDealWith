using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using YTS.Log;

namespace CodingSupportLibrary.Test
{
    [TestClass]
    public class TestISupport
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logEncoding = new UTF8Encoding(false);
            var logFile = ILogExtend.GetLogFilePath("TestISupport");
            log = new FilePrintLog(logFile, logEncoding);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encodingNames = new string[] {
                "utf-8",
                "GBK",
                //"IBM01145",
                //"IBM285",
                //"iso-8859-2",
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_FindFileEncoding()
        {

        }
    }
}
