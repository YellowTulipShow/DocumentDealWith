using System;
using System.IO;
using System.Text;
using System.Linq;
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
                string rootDire = context.ParseResult.GetValueForOption(globalOptions.RootDire);
                logArgs["RootDire"] = rootDire;
                if (!Directory.Exists(rootDire))
                {
                    throw new ArgumentNullException($"根目录不存在: {rootDire}");
                }
                log.Info("信息输出", logArgs);

                ConfigHelper configHelper = new ConfigHelper(log, Encoding.UTF8);
                string configPath = context.ParseResult.GetValueForOption(globalOptions.Config);
                configPath = ToAbsPath(configPath, rootDire);
                logArgs["configPath"] = configPath;
                Configs config = configHelper.ReadConfigs(configPath);
                if (config == null)
                {
                    throw new ArgumentNullException($"配置内容读取为空: {configPath}");
                }

                string[] filePaths = context.ParseResult.GetValueForOption(globalOptions.Files);
                for (int i = 0; i < filePaths.Length; i++)
                {
                    filePaths[i] = ToAbsPath(filePaths[i], rootDire);
                }
                filePaths = filePaths.Where(b => !string.IsNullOrEmpty(b)).ToArray();
                logArgs["filePaths"] = filePaths;

                string direPath = context.ParseResult.GetValueForOption(globalOptions.Path);
                direPath = ToAbsPath(direPath, rootDire);
                logArgs["direPath"] = direPath;

                bool direPathIsRecurse = context.ParseResult.GetValueForOption(globalOptions.PathIsRecurse);
                logArgs["direPathIsRecurse"] = direPathIsRecurse;

                string textFilePath = context.ParseResult.GetValueForOption(globalOptions.FileText);
                textFilePath = ToAbsPath(textFilePath, rootDire);
                logArgs["textFilePath"] = textFilePath;

                T m = new T
                {
                    Config = config,
                    RootDire = new DirectoryInfo(rootDire),
                    Files = filePaths,
                    Path = string.IsNullOrEmpty(direPath) ? null : new DirectoryInfo(direPath),
                    PathIsRecurse = direPathIsRecurse,
                    FileText = string.IsNullOrEmpty(textFilePath) ? null : new FileInfo(textFilePath),
                };
                return m;
            }
            catch (Exception ex)
            {
                log.Error("转换命令参数", ex, logArgs);
                throw ex;
            }
        }

        private static string ToAbsPath(string path, string root)
        {
            path = path?.Trim();
            root = root?.Trim();
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(root))
            {
                return null;
            }

            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.Combine(root, path);
            }
        }
    }
}
