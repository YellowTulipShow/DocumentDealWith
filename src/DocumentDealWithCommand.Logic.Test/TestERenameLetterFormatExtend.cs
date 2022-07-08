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
    public class TestERenameLetterFormatExtend
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestERenameLetterFormatExtend");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_ToChars()
        {
            Check("abcdefghijklmnopqrstuvwxyz".ToArray(),
                ERenameLetterFormatExtend.ToChars(ERenameLetterFormat.Lower));
            Check("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray(),
                ERenameLetterFormatExtend.ToChars(ERenameLetterFormat.Upper));
            Check("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray(),
                ERenameLetterFormatExtend.ToChars(ERenameLetterFormat.LowerAndUpper));
            Check("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToArray(),
                ERenameLetterFormatExtend.ToChars(ERenameLetterFormat.UpperAndLower));
        }
        private void Check(char[] expected, char[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
