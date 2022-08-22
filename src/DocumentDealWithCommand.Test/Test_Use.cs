using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Text;
using System.Collections.Generic;

using YTS.Log;
using YTS.ConsolePrint;
using YTS.CodingSupportLibrary;

using CommandParamUse;

using DocumentDealWithCommand.Test.OverrideClass;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Test
{
    [TestClass]
    public class Test_Use
    {
        private class ExeFunc : ITestAllExecute
        {
            private readonly IDictionary<string, object> dict;
            public ExeFunc()
            {
                dict = new Dictionary<string, object>();
            }

            public int OnExecute(CommandParameters_Content_Encoding param)
            {
                JugeTestFunc(param);
                return 0;
            }
            public void SetTestFunc<T>(Action<T> testFunc)
            {
                Type type = typeof(T);
                string type_name = type.FullName;
                dict[type_name] = testFunc;
            }
            private void JugeTestFunc<T>(T param)
            {
                Type type = param.GetType();
                string type_name = type.FullName;
                object dict_testFunc = dict[type_name];
                Action<T> testFunc = (Action<T>)dict_testFunc;
                testFunc.Invoke(param);
            }

            public int OnExecute(CommandParameters_Content_NewLine param)
            {
                JugeTestFunc(param);
                return 0;
            }

            public int OnExecute(ParamRenameWhole param)
            {
                JugeTestFunc(param);
                return 0;
            }

            public int OnExecute(ParamRenameAddOrDelete param)
            {
                JugeTestFunc(param);
                return 0;
            }

            public int OnExecute(ParamRenameReplace param)
            {
                JugeTestFunc(param);
                return 0;
            }
        }

        [TestMethod]
        public void Test_Content_Encoding()
        {
            ILog log = new FilePrintLog(ILogExtend.GetLogFilePath("Program"), Encoding.UTF8)
                .Connect(new ConsolePrintLog());

            void test<T>(string[] args, Action<T> jugeFunc)
            {
                ExeFunc exe = new ExeFunc();
                exe.SetTestFunc(jugeFunc);
                ICommandRoot rootCmd = new Test_MainCommand(log, exe);
                int code = rootCmd.OnParser(args, log);
                Assert.AreEqual(0, code);
            };

            string[] args;

            args = @"content encode --console PowerShell --root D:\ --target UTF16_BigEndian --files C:\\code.txt".Split(" ");
            test<CommandParameters_Content_Encoding>(args, param =>
            {
                Assert.AreEqual(EConsoleType.PowerShell, param.ConsoleType);
                Assert.AreEqual("D:\\", param.RootDire.FullName);
                Assert.IsNotNull(param.NeedHandleFileInventory);
                Assert.IsTrue(param.NeedHandleFileInventory.Length == 1);
                Assert.AreEqual(@"C:\code.txt", param.NeedHandleFileInventory[0].FullName);
                Assert.AreEqual(ESupportEncoding.UTF16_BigEndian, param.Target);
            });
            args = @"content encode --console PowerShell --root D:\ --target UTF16_BigEndian --files D:\template\test\1.txt".Split(" ");
            test<CommandParameters_Content_Encoding>(args, param =>
            {
                Assert.AreEqual(EConsoleType.PowerShell, param.ConsoleType);
                Assert.AreEqual("D:\\", param.RootDire.FullName);
                Assert.IsNotNull(param.NeedHandleFileInventory);
                Assert.IsTrue(param.NeedHandleFileInventory.Length == 1);
                Assert.AreEqual(@"D:\template\test\1.txt", param.NeedHandleFileInventory[0].FullName);
                Assert.AreEqual(ESupportEncoding.UTF16_BigEndian, param.Target);
            });
            args = @"content encode --console PowerShell --root D:\ --target UTF16_BigEndian --files /D/template/test/1.txt".Split(" ");
            test<CommandParameters_Content_Encoding>(args, param =>
            {
                Assert.AreEqual(EConsoleType.PowerShell, param.ConsoleType);
                Assert.AreEqual("D:\\", param.RootDire.FullName);
                Assert.IsNotNull(param.NeedHandleFileInventory);
                Assert.IsTrue(param.NeedHandleFileInventory.Length == 1);
                Assert.AreEqual(@"D:\D\template\test\1.txt", param.NeedHandleFileInventory[0].FullName);
                Assert.AreEqual(ESupportEncoding.UTF16_BigEndian, param.Target);
            });
            args = (@"content encode --console PowerShell" +
                    @" --root D:\ --target UTF16_BigEndian" +
                    @" --files /D/template/test/1.txt /D/template/test/Mall_UserInfoController.cs"
                    ).Split(" ");
            test<CommandParameters_Content_Encoding>(args, param =>
            {
                Assert.AreEqual(EConsoleType.PowerShell, param.ConsoleType);
                Assert.AreEqual("D:\\", param.RootDire.FullName);
                Assert.IsNotNull(param.NeedHandleFileInventory);
                Assert.IsTrue(param.NeedHandleFileInventory.Length == 2);
                Assert.AreEqual(@"D:\D\template\test\1.txt", param.NeedHandleFileInventory[0].FullName);
                Assert.AreEqual(@"D:\D\template\test\Mall_UserInfoController.cs", param.NeedHandleFileInventory[1].FullName);
                Assert.AreEqual(ESupportEncoding.UTF16_BigEndian, param.Target);
            });

            args = (@"rename --console PowerShell --root D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename" +
                    @" --path ./ --recurse=true" +
                    @" --rule #_#" +
                    @" --is-preview=true --preview-column=20"
                    ).Split(" ");
            test<ParamRenameWhole>(args, param =>
            {
                Assert.AreEqual(EConsoleType.PowerShell, param.ConsoleType);
                Assert.AreEqual(@"D:\Work\YTS.ZRQ\DocumentDealWith\_test\rename", param.RootDire.FullName);
                Assert.IsNotNull(param.NeedHandleFileInventory);
                Assert.IsTrue(param.NeedHandleFileInventory.Length > 0);
                Assert.AreEqual(true, param.IsPreview);
                Assert.AreEqual((uint)20, param.PreviewColumnCount);
                Assert.AreEqual("#_#", param.NamingRules);
            });
        }
    }
}
