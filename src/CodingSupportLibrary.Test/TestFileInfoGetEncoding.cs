using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Text;

using YTS.Log;

namespace CodingSupportLibrary.Test
{
    [TestClass]
    public class TestFileInfoGetEncoding
    {
        private const string Text_zh = @"
///撒旦<summary>
JSON在线 | JSON解析格式化—SO JSON在线工具
打款给几^@个姐姐*#@(给几个i姐姐给个姐姐格局
<>33,32,2.52.
而立+*-+即给
善良的看看/*-+
";
        private const string Text_en = @"
/// sLiellekg <summary>
/// lsdijfliajlsdjlfajg
*d-f--23+f+ddfjiljsig*OUO8+
u3li23k(&@L@)*!??SOF_W@_L@|>:OL~~!``;fdi12+d-3+lsieig
";

        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logEncoding = new UTF8Encoding(false);
            var logFile = ILogExtend.GetLogFilePath("TestISupport");
            log = new FilePrintLog(logFile, logEncoding);
            UseExtend.SupportCodePages();
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_FindFileEncoding()
        {
            Encoding[] encodings_zh = new Encoding[] {
                Encoding.GetEncoding("utf-16"),
                Encoding.GetEncoding("unicodeFFFE"),
                Encoding.GetEncoding("utf-32"),
                Encoding.GetEncoding("utf-7"),
                Encoding.GetEncoding("utf-8"),
                Encoding.GetEncoding("GBK"),
            };
            foreach (var item in encodings_zh)
            {
                Test_FindFileEncoding(Text_zh.Trim(), item);
            }
            Encoding[] encodings_en = new Encoding[] {
                Encoding.GetEncoding("us-ascii"),
                Encoding.GetEncoding("iso-8859-1"),
            };
            foreach (var item in encodings_en)
            {
                Test_FindFileEncoding(Text_en.Trim(), item);
            }
        }
        private void Test_FindFileEncoding(string text, Encoding encoding)
        {
            string name = encoding.EncodingName;
            FileInfo codeFile = new FileInfo(Path.Combine(Environment.CurrentDirectory,
                $"./codefiles/{name}.txt"));
            if (!codeFile.Directory.Exists)
            {
                codeFile.Directory.Create();
            }
            File.WriteAllText(codeFile.FullName, text, encoding);
            
            // 开始判断内容编码逻辑
            Encoding fileEncoding = codeFile.GetEncoding();
            Assert.IsNotNull(fileEncoding);
            Assert.AreEqual(encoding.BodyName, fileEncoding.BodyName);
            Assert.AreEqual(encoding.HeaderName, fileEncoding.HeaderName);
            Assert.AreEqual(encoding.WebName, fileEncoding.WebName);
            Assert.AreEqual(encoding.EncodingName, fileEncoding.EncodingName);
            string fileContent = File.ReadAllText(codeFile.FullName, fileEncoding);
            Assert.AreEqual(text, fileContent);
        }
    }
}
