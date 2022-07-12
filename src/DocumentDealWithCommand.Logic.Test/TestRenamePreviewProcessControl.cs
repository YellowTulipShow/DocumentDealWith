using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Text;

using YTS.Log;

using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestRenamePreviewProcessControl
    {
        private ILog log;
        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestRenamePreviewProcessControl");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_AnalysisMoveArrayExpression()
        {
            var main = new RenamePreviewProcessControl(log, null, null);
            RenamePreviewProcessControl.AnalysisMoveArrayExpressionResult result;
            void Test(string expression, uint starts, string position_str)
            {
                uint[] position = position_str
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(b =>
                    {
                        uint.TryParse(b, out uint v);
                        return v;
                    })
                    .ToArray();
                result = main.AnalysisMoveArrayExpression(expression);
                Assert.IsTrue(result.IsSuccess);
                Assert.AreEqual(starts, result.TargetIndex);
                Check(position, result.NeedOperationItemPositionIndex);
            }
            Test("7>2", 2, "7");
            Test("7..9>2", 2, "7,8,9");
            Test("5,7,9>2", 2, "5,7,9");
            Test("2,6>4", 4, "2,6");
            Test("2,4,6..8>0", 0, "2,4,6,7,8");
            Test("2,4,6..8>99", 99, "2,4,6,7,8");
            Test("6..8,2,4>4", 4, "2,4,6,7,8");
        }

        [TestMethod]
        public void Test_MoveArray()
        {
            var main = new RenamePreviewProcessControl(log, null, null);
            void Test(string source, string result, string expression)
            {
                char[] srouceArray = source.ToCharArray();
                var analysisResult = main.AnalysisMoveArrayExpression(expression);
                Assert.IsTrue(analysisResult.IsSuccess);
                char[] rlist = main.MoveUserIndexItems(srouceArray, analysisResult).ToArray();
                Check(result.ToCharArray(), rlist);
            }
            const string str = "123456789";
            Test(str, "123456789", "1..9>1");
            Test(str, "172345689", "7>2");
            Test(str, "178923456", "7..9>2");
            Test(str, "157923468", "5,7,9>2");
            Test(str, "134265789", "2,6>4");
            Test(str, "135246789", "2,4,6>4");
            Test(str, "135246789", "2,4,6..8>4");
            Test(str, "124678359", "2,4,6..8>2");
            Test(str, "135924678", "2,4,6..8>9");
            Test(str, "135924678", "2,4,6..8>8");
            Test(str, "135924678", "2,4,6..8>7");
            Test(str, "135924678", "2,4,6..8>6");
            Test(str, "135924678", "2,4,6..8>5");
            Test(str, "135246789", "2,4,6..8>4");
            Test(str, "132467859", "2,4,6..8>3");
            Test(str, "124678359", "2,4,6..8>2");
            Test(str, "246781359", "2,4,6..8>1");
            Test(str, "246781359", "2,4,6..8>0");
        }

        private void Check<T>(T[] expected, T[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void Test_RemoveUserIndexItem()
        {
            var main = new RenamePreviewProcessControl(log, null, null);
            void Test(string source, string result, uint index)
            {
                char[] rlist = main.RemoveUserIndexItem(source.ToCharArray(), index).ToArray();
                Check(result.ToCharArray(), rlist);
            }
            const string str = "123456789";
            Test(str, "23456789", 0);
            Test(str, "23456789", 1);
            Test(str, "13456789", 2);
            Test(str, "12456789", 3);
            Test(str, "12345689", 7);
            Test(str, "12345679", 8);
            Test(str, "12345678", 9);
            Test(str, "12345678", 10);
        }
    }
}
