using System;
using System.IO;
using System.Linq;
using System.Text;
using System.CommandLine;

using YTS.Log;
using YTS.ConsolePrint;

using Newtonsoft.Json;

using CommandParamUse;
using CommandParamUse.Implementation;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.Logic.Implementation;

namespace DocumentDealWithCommand.ParamConfigs
{
    /// <inheritdoc/>
    public abstract class AbsBasicParamConfig<P> : AddParamConfigDefalutValue<P> where P : BasicCommandParameters, new()
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;

        /// <inheritdoc/>
        public AbsBasicParamConfig(ILog log) : base()
        {
            this.log = log;

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

        /// <inheritdoc/>
        public override P ParameterProcess(P param)
        {
            param = base.ParameterProcess(param);
            param = FillBasicCommandParameters(param, param.GlobalOptions);
            return param;
        }

        /// <summary>
        /// 赋值转换为基础参数
        /// </summary>
        private T FillBasicCommandParameters<T>(T m, GlobalOptionsValue gOValue)
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

        private string ToAbsPath(string path, string root)
        {
            path = path?.Trim();
            root = root?.Trim();
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(root))
                return null;
            return Path.IsPathRooted(path) ?
                path :
                Path.Combine(root, path);
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
        public virtual string[] GetConfigAllowExtensions(Configs config)
        {
            return config.AllowExtension.Global;
        }
    }
}
