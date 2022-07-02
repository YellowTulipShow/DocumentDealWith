using System;
using System.IO;
using System.Linq;
using System.Text;
using System.CommandLine;
using System.CommandLine.Invocation;

using Newtonsoft.Json;

using YTS.Log;
using YTS.ConsolePrint;

using DocumentDealWithCommand.Logic.Implementation;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 抽象基类实现
    /// </summary>
    public abstract class AbsSubCommand : ISubCommand
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// 全局配置项
        /// </summary>
        protected readonly GlobalOptions globalOptions;

        /// <summary>
        /// 实例化子命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="globalOptions">全局配置项</param>
        public AbsSubCommand(ILog log, GlobalOptions globalOptions)
        {
            this.log = log;
            this.globalOptions = globalOptions;
        }

        /// <inheritdoc/>
        public abstract Command GetCommand();

        /// <summary>
        /// 转为命令参数
        /// </summary>
        /// <typeparam name="T">命令配置, 基于全局配置</typeparam>
        /// <param name="context">解析上下文</param>
        /// <returns>命令配置</returns>
        public virtual T ToCommandParameters<T>(InvocationContext context) where T : BasicCommandParameters, new()
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                EConsoleType consoleType = context.ParseResult.GetValueForOption(globalOptions.ConsoleType);
                T m = new T
                {
                    Config = null,
                    NeedHandleFileInventory = null,
                    ConsoleType = consoleType,
                    Print = EConsoleTypeExtend.ToIPrintColor(consoleType),
                };

                string rootDire = context.ParseResult.GetValueForOption(globalOptions.RootDire);
                logArgs["RootDire"] = rootDire;
                if (!Directory.Exists(rootDire))
                {
                    throw new ArgumentNullException($"根目录不存在: {rootDire}");
                }
                m.Print.Write($"当前目录: ");
                m.Print.WriteLine(rootDire, EPrintColor.Green);

                string configPath = context.ParseResult.GetValueForOption(globalOptions.Config);
                configPath = ToAbsPath(configPath, rootDire);
                logArgs["configPath"] = configPath;
                m.Config = ReadConfigs(configPath, m.Print);
                if (m.Config == null)
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

                var allowExtensions = GetConfigAllowExtensions(m.Config);
                var calcFileInventory = new CalcCanOperationFileInventory(log,
                    new DirectoryInfo(rootDire), allowExtensions);
                calcFileInventory.Append(filePaths);
                calcFileInventory.Append(string.IsNullOrEmpty(textFilePath) ? null :
                    new FileInfo(textFilePath));
                calcFileInventory.Append(string.IsNullOrEmpty(direPath) ? null :
                    new DirectoryInfo(direPath), direPathIsRecurse);
                m.NeedHandleFileInventory = calcFileInventory.GetResults();
                if (m.NeedHandleFileInventory == null || m.NeedHandleFileInventory.Length <= 0)
                {
                    throw new ArgumentNullException("rinventory", "可操作文件清单为空列表, 请检查传入参数!");
                }
                return m;
            }
            catch (Exception ex)
            {
                log.Error("转换命令参数出错", ex, logArgs);
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

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径指定</param>
        /// <param name="print">打印输出接口</param>
        /// <returns>配置内容</returns>
        private Configs ReadConfigs(string configFilePath, IPrintColor print)
        {
            Encoding encoding = Encoding.UTF8;
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
            FileInfo file = new FileInfo(configFilePath);
            if (!file.Exists)
            {
                file.Create().Close();
                Configs defaultConfigs = Configs.GetDefaultConfigs();
                string json = JsonConvert.SerializeObject(defaultConfigs, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, encoding);
                print.Write($"配置文件不存在, 自动创建默认项: ");
                print.WriteLine(file.FullName, EPrintColor.Red);
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, encoding);
            var config = JsonConvert.DeserializeObject<Configs>(content);
            print.Write($"获取配置文件成功: ");
            print.WriteLine(file.FullName, EPrintColor.Green);
            return config;
        }

        /// <summary>
        /// 获取配置: 允许的文件名队列
        /// </summary>
        protected abstract string[] GetConfigAllowExtensions(Configs config);
    }
}
