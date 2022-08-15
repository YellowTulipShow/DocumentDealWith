using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Text;

using CommandParamUse;

using DocumentDealWithCommand.SubCommand;

using YTS.ConsolePrint;

namespace DocumentDealWithCommand
{
    public class MainCommand : IRootCommand<GlobalOptionsValue>
    {
        public string GetDescription()
        {
            return "文档文件相关操作命令";
        }

        public IExecute<GlobalOptionsValue> GetExecute()
        {
            return null;
        }

        public IParamConfig<GlobalOptionsValue> GetParamConfig()
        {
            return new MainCommandParamConfig();
        }

        public IEnumerable<ISubCommand<GlobalOptionsValue>> GetSubCommands()
        {
            ISubCommand<GlobalOptionsValue> sub = new SubCommand_Content();
            yield return sub;
        }
    }

    public class MainCommandParamConfig : IParamConfig<GlobalOptionsValue>
    {
        private Option<string> GetOption_Config()
        {
            var option = new Option<string>(
                aliases: new string[] { "--config" },
                description: "配置文件读取路径",
                getDefaultValue: () =>
                {
                    // 当前用户配置地址
                    string dire = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string file = Path.Combine(dire, ".command_document_dealwith_config.json");
                    return file;
                })
            {
                Arity = ArgumentArity.ExactlyOne,
            };
            return option;
        }
        private Option<string> GetOption_RootDire()
        {
            var option = new Option<string>(
                aliases: new string[] { "--root" },
                description: "操作文件所属路径",
                getDefaultValue: () => Environment.CurrentDirectory)
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }
        private Option<EConsoleType> GetOption_ConsoleType()
        {
            var option = new Option<EConsoleType>(
                aliases: new string[] { "--console" },
                description: "输出打印配置, 控制台类型",
                getDefaultValue: () => EConsoleTypeExtend.GetDefalutConsoleType())
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }
        private Option<string[]> GetOption_Files()
        {
            var option = new Option<string[]>(
                aliases: new string[] { "--files" },
                description: "操作文件路径")
            {
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true,
            };
            return option;
        }
        private Option<string> GetOption_Path()
        {
            var option = new Option<string>(
                aliases: new string[] { "--path" },
                description: "操作文件所属路径")
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }
        private Option<bool> GetOption_Recurse()
        {
            var option = new Option<bool>(
                aliases: new string[] { "--recurse" },
                description: "是否递归查询, 用于与 --path 参数配合查询")
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }
        private Option<string> GetOption_FileText()
        {
            var option = new Option<string>(
                aliases: new string[] { "--txt" },
                description: "操作文件组合清单文件路径")
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }

        GlobalOptionsValue IParamConfig<GlobalOptionsValue>.CreateParam()
        {
            return new GlobalOptionsValue();
        }

        IEnumerable<IInputOption<GlobalOptionsValue>> IParamConfig<GlobalOptionsValue>.GetGlobalInputs()
        {
            yield return new InputOption<string, GlobalOptionsValue>(
                GetOption_Config(),
                (param, value) => param.Config = value);
            yield return new InputOption<string, GlobalOptionsValue>(
                GetOption_RootDire(),
                (param, value) => param.RootDire = value);
            yield return new InputOption<EConsoleType, GlobalOptionsValue>(
                GetOption_ConsoleType(),
                (param, value) => param.ConsoleType = value);
            yield return new InputOption<string[], GlobalOptionsValue>(
                GetOption_Files(),
                (param, value) => param.Files = value);
            yield return new InputOption<string, GlobalOptionsValue>(
                GetOption_Path(),
                (param, value) => param.Path = value);
            yield return new InputOption<bool, GlobalOptionsValue>(
                GetOption_Recurse(),
                (param, value) => param.PathIsRecurse = value);
            yield return new InputOption<string, GlobalOptionsValue>(
                GetOption_FileText(),
                (param, value) => param.FileText = value);
        }

        IEnumerable<IInputOption<GlobalOptionsValue>> IParamConfig<GlobalOptionsValue>.GetInputs()
        {
            return null;
        }
    }
}
