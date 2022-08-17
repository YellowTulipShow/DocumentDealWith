using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

using YTS.Log;

using CommandParamUse.Implementation;

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
        /// <param name="args">用户传入的命令行参数</param>
        /// <param name="root">根命令配置项</param>
        /// <param name="log">日志接口</param>
        /// <returns>执行返回编码</returns>
        public static int OnParser(this ICommandRoot root, string[] args, ILog log)
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
        /// 填充命令内容
        /// </summary>
        /// <typeparam name="TCmd">命令类型</typeparam>
        /// <param name="cmd">命令实体</param>
        /// <param name="root">创建命令对象</param>
        /// <returns>填充完成的命令实体</returns>
        public static TCmd FillCommandContent<TCmd>(this TCmd cmd, ICommand<TCmd> root)
            where TCmd : Command
        {
            // 创建子命令对象
            IEnumerable<ICommandSub> subCommands = root.GetSubCommands();
            if (subCommands != null)
            {
                foreach (ICommandSub sub in subCommands)
                {
                    Command subCmd = sub.GetCommand();
                    cmd.AddCommand(subCmd);
                }
            }
            return cmd;
        }

        /// <summary>
        /// 填充参数型命令内容
        /// </summary>
        /// <typeparam name="TCmd">命令类型</typeparam>
        /// <typeparam name="TParam">参数类型</typeparam>
        /// <param name="cmd">命令实体</param>
        /// <param name="root">创建命令对象</param>
        /// <returns>填充完成的命令实体</returns>
        public static TCmd FillParamCommandContent<TCmd, TParam>(this TCmd cmd, ICommandParam<TCmd, TParam> root)
            where TCmd : Command
            where TParam : IParam
        {
            cmd = cmd.FillCommandContent(root);

            IDictionary<string, object> logArgs = new Dictionary<string, object>
            {
                ["TC.TypeName"] = typeof(TCmd).Name,
                ["P.TypeName"] = typeof(TParam).Name,
                ["TC:Command(cmd).Type"] = cmd?.GetType()?.Name,
                ["P:IParam(root).Type"] = root?.GetType()?.Name
            };

            IExecute<TParam> exe = root.GetExecute();
            logArgs["IExecute(exe).Type"] = exe?.GetType()?.Name;
            IParamConfig<TParam> pConfig = root.GetParamConfig();
            logArgs["IParamConfig(pConfig).Type"] = exe?.GetType()?.Name;
            if (pConfig != null)
            {
                FillCommandInputOptions(cmd, pConfig.GetGlobalInputs(), (cmd, option) => cmd.AddGlobalOption(option));
                if (exe != null)
                {
                    FillCommandInputOptions(cmd, pConfig.GetInputs(), (cmd, option) => cmd.AddGlobalOption(option));
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

        private static void FillCommandInputOptions<TC, P>(TC cmd, IEnumerable<IInputOption<P>> inputs, Action<TC, Option> fillFunc)
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

        /// <summary>
        /// 设置参数输入配置全局项
        /// </summary>
        /// <typeparam name="TData">输入字段数据类型</typeparam>
        /// <typeparam name="TParam">参数类型</typeparam>
        /// <param name="option">命令行解析参数配置项</param>
        /// <param name="addConfig">添加类型配置项</param>
        /// <param name="fillParamMethod">填充值方法</param>
        public static void SetGlobal<TData, TParam>(this Option<TData> option, AddParamConfig<TParam> addConfig, Action<TParam, TData> fillParamMethod) where TParam : IParam
        {
            IInputOption<TParam> inputOption = new InputOption<TData, TParam>(option, fillParamMethod);
            addConfig.AddInputOption(inputOption, isGlobal: true);
        }

        /// <summary>
        /// 设置参数输入配置项
        /// </summary>
        /// <typeparam name="TData">输入字段数据类型</typeparam>
        /// <typeparam name="TParam">参数类型</typeparam>
        /// <param name="option">命令行解析参数配置项</param>
        /// <param name="addConfig">添加类型配置项</param>
        /// <param name="fillParamMethod">填充值方法</param>
        public static void Set<TData, TParam>(this Option<TData> option, AddParamConfig<TParam> addConfig, Action<TParam, TData> fillParamMethod) where TParam : IParam
        {
            IInputOption<TParam> inputOption = new InputOption<TData, TParam>(option, fillParamMethod);
            addConfig.AddInputOption(inputOption, isGlobal: false);
        }
    }
}
