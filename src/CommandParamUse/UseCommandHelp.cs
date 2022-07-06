using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;

using YTS.Log;

namespace CommandParamUse
{
    /// <summary>
    /// 调用命令解析程序实现帮助类
    /// </summary>
    public class UseCommandHelp
    {
        private readonly ILog log;

        /// <summary>
        /// 实例化调用命令解析程序实现帮助类
        /// </summary>
        /// <param name="log">日志接口</param>
        public UseCommandHelp(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 解析执行
        /// </summary>
        /// <param name="args">用户传入的命令行参数</param>
        /// <param name="root">根命令配置项</param>
        /// <returns>执行返回编码</returns>
        public int OnParser(string[] args, IRootCommand root)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["UserInputArgs"] = args;
            logArgs["IRootCommand.Type"] = root.GetType().Name;
            try
            {
                RootCommand cmd = root.GetCommand();
                logArgs["RootCommand.Name"] = cmd.Name;
                logArgs["RootCommand.Description"] = cmd.Description;
                IContent content = root.GetContent();
                logArgs["ICommandContent.Type"] = content.GetType().Name;
                cmd = ConfigContent(cmd, content);
                return cmd.Invoke(args);
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                return -1;
            }
        }

        private TC ConfigContent<TC>(TC cmd, IContent content) where TC : Command
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["Command.Name"] = cmd.Name;
            logArgs["Command.Description"] = cmd.Description;
            logArgs["ICommandContent(content).Type"] = content.GetType().Name;
            try
            {
                IEnumerable<ISubCommand> subCommands = content.GetSubCommands()?.ToArray();
                if (subCommands != null)
                {
                    // 创建子命令对象
                    foreach (ISubCommand sub in subCommands)
                    {
                        logArgs["ISubCommand.Type"] = sub.GetType().Name;
                        Command subCmd = sub.GetCommand();
                        logArgs["SubCommand.Name"] = subCmd.Name;
                        logArgs["SubCommand.Description"] = subCmd.Description;
                        IContent subContent = sub.GetContent();
                        logArgs["ICommandContent(subContent).Type"] = content.GetType().Name;
                        subCmd = ConfigContent(subCmd, subContent);
                        cmd.AddCommand(subCmd);
                    }
                }
                IExecute exe = content.GetExecute();
                logArgs["IExecute(exe).Type"] = exe.GetType().Name;
                IEnumerable<IInput> gOptions = exe.GetGlobalInputs()?.ToArray();
                if (gOptions != null)
                {
                    foreach (var item in gOptions)
                    {
                        var option = item.GetOption();
                        cmd.AddGlobalOption(option);
                    }
                }
                IEnumerable<IInput> options = exe.GetInputs()?.ToArray();
                if (options != null)
                {
                    foreach (var item in options)
                    {
                        var option = item.GetOption();
                        cmd.AddOption(item.GetOption());
                    }
                }
                if (exe.IsExecute())
                {
                    cmd.SetHandler((context) =>
                    {
                        try
                        {
                            context.ExitCode = exe.OnExecute(context);
                        }
                        catch (Exception ex)
                        {
                            log.Error($"{cmd.Description} - 执行出错", ex, logArgs);
                            context.ExitCode = 1;
                        }
                    });
                }
                return cmd;
            }
            catch (Exception ex)
            {
                log.Error("填充命令内容出错", ex, logArgs);
                throw ex;
            }
        }
    }
}
