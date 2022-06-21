﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.CommandLine;
using System.CommandLine.Parsing;

using YTS.Log;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

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

        private Option<string> GetOption_Config()
        {
            var option = new Option<string>(
                aliases: new string[] { "-c", "--Config" },
                description: "配置文件读取路径",
                getDefaultValue: () =>
                {
                    // 当前用户配置地址
                    string dire = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string file = Path.Combine(dire, ".command_gitcheck_config.json");
                    return file;
                })
            {
                Arity = ArgumentArity.ExactlyOne,
            };
            return option;
        }

        private Option<string[]> GetOption_Files()
        {
            var option = new Option<string[]>(
                aliases: new string[] { "-f", "--Files" },
                description: "操作文件路径",
                parseArgument: result => result.Tokens
                    .Select(b => b.Value)
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
