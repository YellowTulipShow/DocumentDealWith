using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using YTS.Log;
using YTS.ConsolePrint;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestChangeFileNameHelp : IPrintColor
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

            DirectoryInfo rootDire = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory,
                $"./changeFileNameDire"));
            if (!rootDire.Exists)
                rootDire.Create();
            var r = new Random();
            string chars = "0123456789qwertyuiopasdfghjklzxcvbnm_-";
            char getChar() {
                return chars[r.Next(0, chars.Length)];
            }
            int add_count = r.Next(5, 30);
            for (int i = 0; i < add_count; i++)
            {
                string name = $"{getChar()}{getChar()}_{i}.txt";
                FileInfo addFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, name));
                File.WriteAllText(addFile.FullName, name, editEncoding);
            }
            FileInfo[] existsFiles = rootDire.GetFiles();

            IList<HandleRenameResult> rlist = new List<HandleRenameResult>();
            IDictionary<string, string> dict_record = new Dictionary<string, string>();
            for (int i = 0; i < existsFiles.Length; i++)
            {
                FileInfo item = existsFiles[i];
                string target = $"{getChar()}{getChar()}_{i}.txt";
                rlist.Add(new HandleRenameResult()
                {
                    Source = item,
                    Result = target,
                });
                dict_record[target] = item.Name;
            }
            int execute_sign = help.ChangeFileName(null, rootDire, rlist);
            Assert.AreEqual(0, execute_sign);
        }

        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
        }

        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
        }

        public int GetLineCount()
        {
            return 0;
        }

        public void Write(string content)
        {
        }

        public void WriteLine(string content)
        {
        }
    }
}
