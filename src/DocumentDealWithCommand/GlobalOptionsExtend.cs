﻿using DocumentDealWithCommand.Logic.Models;

using System.CommandLine.Invocation;

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
        /// <param name="context">解析上下文</param>
        /// <returns>命令配置</returns>
        public static T ToCommandParameters<T>(this GlobalOptions globalOptions, InvocationContext context)
            where T : AbsBasicCommandParameters, new()
        {
            T m = new T
            {
                Config = context.ParseResult.GetValueForOption(globalOptions.Config),
                Files = context.ParseResult.GetValueForOption(globalOptions.Files),
                Path = context.ParseResult.GetValueForOption(globalOptions.Path),
                PathIsRecurse = context.ParseResult.GetValueForOption(globalOptions.PathIsRecurse),
                FileText = context.ParseResult.GetValueForOption(globalOptions.FileText),
            };
            return m;
        }
    }
}
