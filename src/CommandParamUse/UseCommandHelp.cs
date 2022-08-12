using System;
using System.Linq;
using System.CommandLine;

using YTS.Log;
using System.Collections.Generic;

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
        /// <typeparam name="P">参数类型</typeparam>
        /// <param name="args">用户传入的命令行参数</param>
        /// <param name="root">根命令配置项</param>
        /// <returns>执行返回编码</returns>
        public int OnParser<P>(string[] args, IRootCommand<P> root) where P : IParam
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                logArgs["UserInputArgs"] = args;
                logArgs["IRootCommand.Type"] = root.GetType().Name;
                RootCommand cmd = root.ToCommand();
                cmd = ConfigContent(cmd, root);
                return cmd.Invoke(args);
            }
            catch (ILogParamException ex)
            {
                log.Error("解释命令出错", ex, logArgs, ex.GetParam());
                return -1;
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                return -1;
            }
        }

        private TC ConfigContent<TC, P>(TC cmd, ICommand<P> root)
            where TC : Command
            where P : IParam
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                // 创建子命令对象
                IList<ISubCommand<P>> subCommands = root.GetSubCommands<P>();
                foreach (ISubCommand<P> sub in subCommands)
                {
                    Command subCmd = sub.ToCommand();
                    subCmd = ConfigContent(subCmd, sub);
                    cmd.AddCommand(subCmd);
                }

                IParamConfig<P> paramConfig = root.GetParamConfig();

                IExecute<P> exe = root.GetExecute();
                logArgs["IExecute(exe).Type"] = exe.GetType().Name;
                IInputOption[] options_global = exe.GetGlobalInputs()?.ToArray() ?? new IInputOption[] { };
                foreach (var item in options_global)
                {
                    Option option = item.GetOption();
                    cmd.AddGlobalOption(option);
                }

                if (exe.IsExecute())
                {
                    IInputOption[] options = exe.GetInputs()?.ToArray() ?? new IInputOption[] { };
                    foreach (var item in options)
                    {
                        Option option = item.GetOption();
                        cmd.AddGlobalOption(option);
                    }
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
