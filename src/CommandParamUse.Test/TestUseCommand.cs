using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.CommandLine;
using System.Text;

using YTS.Log;

using CommandParamUse.Implementation;

namespace CommandParamUse.Test
{
    [TestClass]
    public class TestUseCommand
    {
        private ILog log;

        [TestInitialize]
        public void Init()
        {
            var logFile = ILogExtend.GetLogFilePath("TestUseCommand");
            log = new FilePrintLog(logFile, Encoding.UTF8)
                .Connect(new BasicJSONConsolePrintLog());
        }

        public class GlobalParam : IParam
        {
            public string Name { get; set; }
        }
        public class RootCommand : ICommandRoot<GlobalParam>
        {
            public string GetDescription() => "根命令描述";

            public IExecute<GlobalParam> GetExecute() => null;

            public IParamConfig<GlobalParam> GetParamConfig()
            {
                return new RootCommandConfig<GlobalParam>(new GlobalParam());
            }

            public IEnumerable<ICommandSub> GetSubCommands()
            {
                yield return new SubCommand_Rename();
            }
        }
        public class RootCommandConfig<P> : AddParamConfig<P> where P : GlobalParam
        {
            public RootCommandConfig(P param) : base(param)
            {
                new Option<string>(
                    aliases: new string[] { "--name" },
                    description: "名称配置",
                    getDefaultValue: () =>
                    {
                        return "张三";
                    })
                {
                    Arity = ArgumentArity.ExactlyOne,
                }.SetGlobal(this, (param, value) => param.Name = value);
            }
        }
        public class RenameParam : GlobalParam
        {
            public string Target { get; set; }
        }
        public class SubCommand_Rename : ICommandSub<RenameParam>
        {
            public string GetNameSign() => "rename";

            public string GetDescription() => "重命名功能";

            public IExecute<RenameParam> GetExecute()
            {
                return new SubCommand_RenameExecute();
            }

            public IParamConfig<RenameParam> GetParamConfig()
            {
                return new SubCommand_RenameConfig<RenameParam>(new RenameParam());
            }

            public IEnumerable<ICommandSub> GetSubCommands() => null;
        }
        public class SubCommand_RenameConfig<P> : RootCommandConfig<P> where P : RenameParam
        {
            public SubCommand_RenameConfig(P param) : base(param)
            {
                new Option<string>(
                    aliases: new string[] { "--target" },
                    description: "目标配置",
                    getDefaultValue: () =>
                    {
                        return "李四";
                    })
                {
                    Arity = ArgumentArity.ExactlyOne,
                }.Set(this, (param, value) => param.Target = value);
            }
        }
        public class SubCommand_RenameExecute : IExecute<RenameParam>
        {
            public int OnExecute(RenameParam param)
            {
                if (string.IsNullOrEmpty(param.Name) && string.IsNullOrEmpty(param.Target))
                    return 1;
                if (string.IsNullOrEmpty(param.Name))
                    return 2;
                if (string.IsNullOrEmpty(param.Target))
                    return 3;
                if (param.Name == "张三")
                    return 4;
                if (param.Target == "李四")
                    return 5;
                if (param.Name == param.Target)
                    return 6;
                return 0;
            }
        }
        [TestMethod]
        public void Test_Use()
        {
            ICommandRoot<GlobalParam> cmd = new RootCommand();
            void test(int code, string[] args)
            {
                int rcode = cmd.OnParser(args, log);
                Assert.AreEqual(code, rcode);
            };
            test(1, new string[] { });
            test(4, new string[] { "rename" });
            test(5, new string[] { "rename", "--name='AAA'" });
            test(6, new string[] { "rename", "--name='AAA'", "--target='AAA'" });
        }
    }
}
