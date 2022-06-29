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

        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logEncoding = new UTF8Encoding(false);
            var logFile = ILogExtend.GetLogFilePath("TestISupport");
            log = new FilePrintLog(logFile, logEncoding);

            // ���Կ�ʼǰ���ùٷ�����ҳ����, ����֧������GBK
            UseExtend.SupportCodePages();
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_FindFileEncoding()
        {
            // ����
            string[] text_zhs = new string[] {
                @"///����<summary>",
                @"JSON���� | JSON������ʽ����SO JSON���߹���",
                @"������^@�����*#@(������i�����������",
                @"<>33,32,2.52.",
                @"����+*-+����",
                @"�����Ŀ���/*-+",
            };
            Encoding[] encodings_zh = new Encoding[] {
                //Encoding.GetEncoding("utf-16"),
                //Encoding.GetEncoding("unicodeFFFE"),
                //Encoding.GetEncoding("utf-32"),
                //Encoding.GetEncoding("utf-7"),
                Encoding.GetEncoding("utf-8"),
                Encoding.GetEncoding("GBK"),
                new UTF8Encoding(true),
                new UTF8Encoding(false),
            };
            Test_FindFileEncoding(text_zhs, encodings_zh);


            // Ӣ��
            string[] text_ens = new string[] {
                @"/// sLiellekg <summary>",
                @"/// lsdijfliajlsdjlfajg",
                @"* d -f--23+f+ddfjiljsig* OUO8+",
                @"u3li23k(&@L@)*!??SOF_W@_L@|>:OL~~!``;fdi12+d-3+lsieig"
            };
            Encoding[] encodings_en = new Encoding[] {
                //Encoding.GetEncoding("us-ascii"),
                //Encoding.GetEncoding("iso-8859-1"),
            };
            Test_FindFileEncoding(text_ens, encodings_en);
        }
        private void Test_FindFileEncoding(string[] texts, Encoding[] encodings)
        {
            foreach (var encoding in encodings)
            {
                Test_FindFileEncoding(string.Join('\n', texts), encoding);
                foreach (string text in texts)
                {
                    Test_FindFileEncoding(text, encoding);
                }
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

            WriteLogFileContentBytes(codeFile);

            return;
            // ��ʼ�ж����ݱ����߼�
            Encoding fileEncoding = codeFile.GetEncoding();
            Assert.IsNotNull(fileEncoding);
            Assert.AreEqual(encoding.BodyName, fileEncoding.BodyName);
            Assert.AreEqual(encoding.HeaderName, fileEncoding.HeaderName);
            Assert.AreEqual(encoding.WebName, fileEncoding.WebName);
            Assert.AreEqual(encoding.EncodingName, fileEncoding.EncodingName);
            string fileContent = File.ReadAllText(codeFile.FullName, fileEncoding);
            Assert.AreEqual(text, fileContent);
        }
        private void WriteLogFileContentBytes(FileInfo codeFile)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine($"�ļ�: {codeFile.FullName}");
            byte[] datas = File.ReadAllBytes(codeFile.FullName);

            int count = 20;
            int header_len = Math.Ceiling((datas.Length + 1M) / count).ToString().Length;
            string get_line_header(int line) {
                return $"  {line.ToString().PadLeft(header_len, ' ')}:  ";
            };
            str.Append($"{"".PadLeft(get_line_header(0).Length)}");
            for (int i = 0; i < count; i++)
            {
                str.Append($"{i,2}  ");
            }
            for (int i = 0; i < datas.Length; i++)
            {
                if (i % count == 0)
                {
                    int line = (int)Math.Ceiling((decimal)(i / count)) + 1;
                    str.Append($"\n{get_line_header(line)}");
                }
                str.Append($"{datas[i]:X2}  ");
            }
            str.AppendLine(string.Empty);
            log.Info(str.ToString());
        }
    }
}
