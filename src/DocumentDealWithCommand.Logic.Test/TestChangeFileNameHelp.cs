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
            FileInfo[] havefiles = rootDire.GetFiles();
            for (int i = 0; i < havefiles.Length; i++)
                havefiles[i].Delete();

            var r = new Random();
            string chars = "0123456789qwertyuiopasdfghjklzxcvbnm_-";
            char getChar() {
                return chars[r.Next(0, chars.Length)];
            }
            int add_count = r.Next(5, 30);
            for (int i = 0; i < add_count; i++)
            {
                string name = $"{getChar()}{getChar()}_{i}.txt";
                FileInfo addFile = new FileInfo(Path.Combine(rootDire.FullName, name));
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
            int execute_sign = help.ChangeFileName(this, rootDire, rlist);
            Assert.AreEqual(0, execute_sign);
            FileInfo[] filelist = rootDire.GetFiles();
            Assert.AreEqual(dict_record.Count, filelist.Length);
            for (int i = 0; i < filelist.Length; i++)
            {
                FileInfo fileInfo = filelist[i];
                string oldName = null;
                if (dict_record.ContainsKey(fileInfo.Name))
                    oldName = dict_record[fileInfo.Name];
                string repeat_name = fileInfo.Name.Replace("_Repeat", "");
                if (dict_record.ContainsKey(repeat_name))
                    oldName = dict_record[repeat_name];
                Assert.IsNotNull(oldName);
                string content = File.ReadAllText(fileInfo.FullName, editEncoding);
                Assert.AreEqual(oldName, content);
            }
        }

        void IPrintColor.Write(string content, EPrintColor textColor, EPrintColor backgroundColor) { }

        void IPrintColor.WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor) { }

        int IPrint.GetLineCount() { return 0; }

        void IPrint.Write(string content) { }

        void IPrint.WriteLine(string content) { }
    }
}
