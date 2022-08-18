using System;
using System.IO;
using System.CommandLine;

using YTS.ConsolePrint;

using CommandParamUse;
using CommandParamUse.Implementation;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand
{
    /// <inheritdoc/>
    public abstract class MainCommandParamConfig<P> : AddParamConfigDefalutValue<P> where P : BasicCommandParameters, new()
    {
        /// <inheritdoc/>
        public MainCommandParamConfig() : base()
        {
            GetOption_Config().SetGlobal(this, (param, value) => param.GlobalOptions.Config = value);
            GetOption_RootDire().SetGlobal(this, (param, value) => param.GlobalOptions.RootDire = value);
            GetOption_ConsoleType().SetGlobal(this, (param, value) => param.GlobalOptions.ConsoleType = value);
            GetOption_Files().SetGlobal(this, (param, value) => param.GlobalOptions.Files = value);
            GetOption_Path().SetGlobal(this, (param, value) => param.GlobalOptions.Path = value);
            GetOption_Recurse().SetGlobal(this, (param, value) => param.GlobalOptions.PathIsRecurse = value);
            GetOption_FileText().SetGlobal(this, (param, value) => param.GlobalOptions.FileText = value);
        }

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
    }
}
