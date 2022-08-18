using System.IO;
using System.Linq;
using System;
using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using Newtonsoft.Json;

using CommandParamUse;
using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 静态扩展: 全局选项集合
    /// </summary>
    public static class GlobalOptionsValueExtend
    {
        /// <summary>
        /// 赋值转换为基础参数
        /// </summary>
        public static T FillBasicCommandParameters<T>(this T m, GlobalOptionsValue gOValue, ILog log)
            where T : BasicCommandParameters, new()
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                EConsoleType consoleType = gOValue.ConsoleType;
                m.ConsoleType = consoleType;
                m.Print = EConsoleTypeExtend.ToIPrintColor(consoleType);

                string rootDire = gOValue.RootDire;
                logArgs["RootDire"] = rootDire;
                if (!Directory.Exists(rootDire))
                {
                    throw new ILogParamException(logArgs, $"根目录不存在: {rootDire}");
                }
                m.Print.Write($"当前目录: ");
                m.Print.WriteLine(rootDire, EPrintColor.Green);
                m.RootDire = new DirectoryInfo(rootDire);

                string configPath = gOValue.Config;
                configPath = ToAbsPath(configPath, rootDire);
                logArgs["configPath"] = configPath;
                m.Config = ReadConfigs(configPath, m.Print);
                if (m.Config == null)
                {
                    throw new ILogParamException(logArgs, $"配置内容读取为空: {configPath}");
                }

                string[] filePaths = gOValue.Files;
                for (int i = 0; i < filePaths.Length; i++)
                {
                    filePaths[i] = ToAbsPath(filePaths[i], rootDire);
                }
                filePaths = filePaths.Where(b => !string.IsNullOrEmpty(b)).ToArray();
                logArgs["filePaths"] = filePaths;

                string direPath = gOValue.Path;
                direPath = ToAbsPath(direPath, rootDire);
                logArgs["direPath"] = direPath;

                bool direPathIsRecurse = gOValue.PathIsRecurse;
                logArgs["direPathIsRecurse"] = direPathIsRecurse;

                string textFilePath = gOValue.FileText;
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
                    throw new ILogParamException(logArgs, "可操作文件清单为空列表, 请检查传入参数!");
                }
                return m;
            }
            catch (Exception ex)
            {
                throw new ILogParamException(logArgs, "转换命令参数出错!", ex);
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
        private static Configs ReadConfigs(string configFilePath, IPrintColor print)
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
        private static string[] GetConfigAllowExtensions(Configs config)
        {
            return null;
        }
    }
}
