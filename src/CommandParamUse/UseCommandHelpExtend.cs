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
                RootCommand cmd = root.GetCommand();
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

        /// <summary>
        /// 转为实体命令
        /// </summary>
        public static RootCommand ToCommand<P>(this IRootCommand<P> root) where P : IParam
        {
            RootCommand cmd = new RootCommand(root.GetDescription());
            cmd = ConfigContent(cmd, root);
            return cmd;
        }

        /// <summary>
        /// 转为实体命令
        /// </summary>
        public static Command ToCommand<P>(this ISubCommand<P> root) where P : IParam
        {
            Command cmd = new Command(root.GetNameSign(), root.GetDescription());
            cmd = ConfigContent(cmd, root);
            return cmd;
        }

        private static TCmd ConfigContent<TCmd, TParam>(this TCmd cmd, IParamCommand<TCmd, TParam> root)
            where TCmd : Command
            where TParam : IParam
        {
            IDictionary<string, object> logArgs = new Dictionary<string, object>
            {
                ["TC.TypeName"] = typeof(TCmd).Name,
                ["P.TypeName"] = typeof(TParam).Name,
                ["TC:Command(cmd).Type"] = cmd?.GetType()?.Name,
                ["P:IParam(root).Type"] = root?.GetType()?.Name
            };

            // 创建子命令对象
            IEnumerable<ISubCommand> subCommands = root.GetSubCommands();
            if (subCommands != null)
            {
                foreach (ISubCommand sub in subCommands)
                {
                    Command subCmd = sub.GetCommand();
                    cmd.AddCommand(subCmd);
                }
            }
            IExecute<TParam> exe = root.GetExecute();
            logArgs["IExecute(exe).Type"] = exe?.GetType()?.Name;
            IParamConfig<TParam> pConfig = root.GetParamConfig();
            logArgs["IParamConfig(pConfig).Type"] = exe?.GetType()?.Name;
            if (pConfig != null)
            {
                ConfigInputOptions(cmd, pConfig.GetGlobalInputs(), (cmd, option) => cmd.AddGlobalOption(option));
                if (exe != null)
                {
                    ConfigInputOptions(cmd, pConfig.GetInputs(), (cmd, option) => cmd.AddGlobalOption(option));
                    cmd.SetHandler((context) =>
                    {
                        try
                        {
                            TParam param = pConfig.CreateParam();
                            FillParam(context, param, pConfig.GetGlobalInputs());
                            FillParam(context, param, pConfig.GetInputs());
                            context.ExitCode = exe.OnExecute(param);
                        }
                        catch (Exception ex)
                        {
                            context.ExitCode = 1;
                            throw new ILogParamException(logArgs, "生成参数执行逻辑出错", ex);
                        }
                    });
                }
            }
            return cmd;
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
