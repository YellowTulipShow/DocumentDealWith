﻿using System;
using System.IO;
using System.Text;
using System.CommandLine.Invocation;

using YTS.Log;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand
{
    /// <summary>
    /// 静态扩展: 全局配置项获取值
    /// </summary>
    public static class GlobalOptionsExtend
    {
        /// <summary>
        /// 转为命令参数
        /// </summary>
        /// <typeparam name="T">命令配置, 基于全局配置</typeparam>
        /// <param name="globalOptions">全局配置项组合</param>
        /// <param name="log">日志接口</param>
        /// <param name="context">解析上下文</param>
        /// <returns>命令配置</returns>
        public static T ToCommandParameters<T>(this GlobalOptions globalOptions, ILog log, InvocationContext context)
            where T : AbsBasicCommandParameters, new()
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                string RootDire = context.ParseResult.GetValueForOption(globalOptions.RootDire);
                logArgs["RootDire"] = RootDire;

                ConfigHelper configHelper = new ConfigHelper(log, Encoding.UTF8);
                string configPath = context.ParseResult.GetValueForOption(globalOptions.Config);
                logArgs["configPath"] = configPath;
                Configs config = configHelper.ReadConfigs(configPath);

                string[] Files = context.ParseResult.GetValueForOption(globalOptions.Files);
                logArgs["Files"] = Files;
                string Path = context.ParseResult.GetValueForOption(globalOptions.Path);
                logArgs["Path"] = Path;
                bool PathIsRecurse = context.ParseResult.GetValueForOption(globalOptions.PathIsRecurse);
                logArgs["PathIsRecurse"] = PathIsRecurse;
                string FileText = context.ParseResult.GetValueForOption(globalOptions.FileText);
                logArgs["FileText"] = FileText;
                T m = new T
                {
                    Config = config,
                    RootDire = RootDire,
                    Files = Files,
                    Path = Path,
                    PathIsRecurse = PathIsRecurse,
                    FileText = FileText,
                };
                return m;
            }
            catch (Exception ex)
            {
                log.Error("转换命令参数", ex, logArgs);
                throw ex;
            }
        }
    }
}