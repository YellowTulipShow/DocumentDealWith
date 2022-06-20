using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using System.CommandLine;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 命令参数解析器
    /// </summary>
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly IMain main;

        /// <summary>
        /// 实例化 - 命令参数解析器
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="main">主程序接口</param>
        public CommandArgsParser(ILog log, IMain main)
        {
            this.log = log;
            this.main = main;
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
            int code = 0;
            try
            {
                Option<string> configFilePathOption = GetOption_ConfigFilePath();

                RootCommand rootC = new RootCommand("检查目录下所有 Git 仓库状态");
                rootC.AddGlobalOption(configFilePathOption);

                rootC.SetHandler(context =>
                {
                    try
                    {
                        string configFilePath = context.ParseResult.GetValueForOption(configFilePathOption);
                        logArgs["configFilePath"] = configFilePath;
                        main.OnExecute(configFilePath, new CommandOptions()
                        {
                        });
                    }
                    catch (Exception ex)
                    {
                        log.Error("执行程序逻辑出错", ex, logArgs);
                        code = 2;
                    }
                });

                return rootC.Invoke(args);
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                code = 1;
            }
            return code;
        }

        private Option<string> GetOption_ConfigFilePath()
        {
            var option = new Option<string>(
                aliases: new string[] { "-c", "--config" },
                getDefaultValue: () =>
                {
                    // 当前用户配置地址
                    string dire = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string file = Path.Combine(dire, ".command_gitcheck_config.json");
                    return file;
                },
                description: "配置文件读取路径"); ;
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
    }
}
