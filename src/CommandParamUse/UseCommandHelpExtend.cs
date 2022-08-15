using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

using YTS.Log;

namespace CommandParamUse
{
    /// <summary>
    /// 静态扩展类: 调用命令解析程序实现帮助类
    /// </summary>
    public static class UseCommandHelpExtend
    {
        /// <summary>
        /// 转换为实体根命令
        /// </summary>
        /// <typeparam name="P">命令参数类型</typeparam>
        /// <param name="root">根命令对象</param>
        /// <returns>实体根命令</returns>
        public static RootCommand ToCommand<P>(this IRootCommand<P> root) where P : IParam
        {
            return new RootCommand(root.GetDescription());
        }

        /// <summary>
        /// 转换为实体命令
        /// </summary>
        /// <typeparam name="P">命令参数类型</typeparam>
        /// <param name="root">命令对象</param>
        /// <returns>实体命令</returns>
        public static Command ToCommand<P>(this ISubCommand<P> root) where P : IParam
        {
            return new Command(root.GetNameSign(), root.GetDescription());
        }

        /// <summary>
        /// 解析执行
        /// </summary>
        /// <typeparam name="P">参数类型</typeparam>
        /// <param name="args">用户传入的命令行参数</param>
        /// <param name="root">根命令配置项</param>
        /// <param name="log">日志接口</param>
        /// <returns>执行返回编码</returns>
        public static int OnParser<P>(this IRootCommand<P> root, string[] args, ILog log) where P : IParam
        {
            var logArgs = log.CreateArgDictionary();
            try
            {
                logArgs["UserInputArgs"] = args;
                logArgs["IRootCommand.Type"] = root.GetType().Name;
                RootCommand cmd = root.ToCommand();
                cmd = ConfigContent(cmd, root, log);
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

        private static TC ConfigContent<TC, P>(this TC cmd, ICommand<P> root, ILog log)
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
                    subCmd = ConfigContent(subCmd, sub, log);
                    cmd.AddCommand(subCmd);
                }
                IParamConfig<P> paramConfig = root.GetParamConfig();
                IExecute<P> exe = root.GetExecute();
                logArgs["IExecute(exe).Type"] = exe.GetType().Name;
                IParamConfig<P> pConfig = root.GetParamConfig();
                ConfigInputOptions(cmd, pConfig.GetGlobalInputs(), (cmd, option) => cmd.AddGlobalOption(option));
                if (exe != null)
                {
                    ConfigInputOptions(cmd, pConfig.GetInputs(), (cmd, option) => cmd.AddGlobalOption(option));
                    cmd.SetHandler((context) =>
                    {
                        try
                        {
                            P param = pConfig.CreateParam();
                            FillParam(context, param, pConfig.GetGlobalInputs());
                            FillParam(context, param, pConfig.GetInputs());
                            context.ExitCode = exe.OnExecute(param);
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

        private static void ConfigInputOptions<TC, P>(TC cmd, IEnumerable<IInputOption<P>> inputs, Action<TC, Option> fillFunc)
            where TC : Command
            where P : IParam
        {
            if (cmd != null && inputs != null)
            {
                foreach (var item in inputs)
                {
                    Option option = item.GetOption();
                    fillFunc(cmd, option);
                }
            }
        }

        private static void FillParam<P>(InvocationContext context, P param, IEnumerable<IInputOption<P>> inputs) where P : IParam
        {
            if (inputs != null)
            {
                foreach (var item in inputs)
                {
                    item.FillParam(context, param);
                }
            }
        }
    }
}
