using System;
using System.IO;
using System.Text.RegularExpressions;

using System.CommandLine;
using System.CommandLine.Parsing;

using YTS.Log;
using System.Linq;
using DocumentDealWithCommand.Logic.Implementation;
using System.Text;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 命令参数解析器
    /// </summary>
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly ConfigHelper configHelper;

        /// <summary>
        /// 实例化 - 命令参数解析器
        /// </summary>
        /// <param name="log">日志接口</param>
        public CommandArgsParser(ILog log)
        {
            this.log = log;
            configHelper = new ConfigHelper(log, Encoding.UTF8);
        }

        /// <summary>
        /// 解析执行
        /// </summary>
        /// <param name="args">用户传入的命令行参数</param>
        /// <returns>执行返回编码</returns>
        public int OnParser(string[] args)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["CommandInputArgs"] = args;
            try
            {
                GlobalOptions globalOptions = new GlobalOptions()
                {
                    Config = GetOption_Config(),
                    Files = GetOption_Files(),
                    Path = GetOption_Path(),
                    PathIsRecurse = GetOption_Recurse(),
                    FileText = GetOption_FileText(),
                };
                RootCommand rootCMD = new RootCommand("文档文件相关操作命令");
                rootCMD.AddGlobalOption(globalOptions.Config);
                rootCMD.AddGlobalOption(globalOptions.Files);
                rootCMD.AddGlobalOption(globalOptions.Path);
                rootCMD.AddGlobalOption(globalOptions.PathIsRecurse);
                rootCMD.AddGlobalOption(globalOptions.FileText);
                ISubCommand[] subCommands = new ISubCommand[]
                {
                    new SubCommand.SubCommand_Content(log, globalOptions),
                    new SubCommand.SubCommand_Rename(log, globalOptions),
                };
                foreach (var sub in subCommands)
                {
                    Command subCMD = sub.GetCommand();
                    rootCMD.AddCommand(subCMD);
                }
                return rootCMD.Invoke(args);
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                return -1;
            }
        }

        private Option<Configs> GetOption_Config()
        {
            Configs parseArgument(ArgumentResult result)
            {
                // 当前用户配置地址
                string userDirePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string defaultFilePath = Path.Combine(userDirePath, ".command_document_dealwith_config.json");
                string filePath;
                if (result.Tokens.Count == 0)
                {
                    filePath = defaultFilePath;
                }
                else
                {
                    filePath = result.Tokens.Single().Value;
                    if (string.IsNullOrEmpty(filePath))
                    {
                        filePath = defaultFilePath;
                    }
                }
                if (!Regex.IsMatch(filePath, @"\.json$",
                    RegexOptions.ECMAScript | RegexOptions.IgnoreCase))
                {
                    result.ErrorMessage = $"配置文件扩展名异常: {filePath}";
                    return null;
                }
                Configs configs = configHelper.ReadConfigs(filePath);
                return configs;
            }
            var option = new Option<Configs>(
                aliases: new string[] { "-c", "--config" },
                description: "配置文件读取路径",
                parseArgument: parseArgument)
            {
                Arity = ArgumentArity.ExactlyOne,
            };
            return option;
        }

        private Option<FileInfo[]> GetOption_Files()
        {
            var option = new Option<FileInfo[]>(
                aliases: new string[] { "-f", "--Files" },
                description: "操作文件路径",
                parseArgument: result => result.Tokens
                    .Select(b => new FileInfo(b.Value))
                    .ToArray())
            {
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true,
            };
            return option;
        }

        private Option<DirectoryInfo> GetOption_Path()
        {
            var option = new Option<DirectoryInfo>(
                aliases: new string[] { "-p", "--Path" },
                description: "操作文件所属路径",
                parseArgument: result => new DirectoryInfo(result.Tokens.Single().Value))
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }

        private Option<bool> GetOption_Recurse()
        {
            var option = new Option<bool>(
                aliases: new string[] { "-r", "--Recurse" },
                getDefaultValue => false,
                description: "是否递归查询用于与 --Path 参数配合查询")
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }

        private Option<FileInfo> GetOption_FileText()
        {
            var option = new Option<FileInfo>(
                aliases: new string[] { "--FileText" },
                description: "操作文件组合清单文件路径",
                parseArgument: result => new FileInfo(result.Tokens.Single().Value))
            {
                Arity = ArgumentArity.ZeroOrOne,
            };
            return option;
        }
    }
}
