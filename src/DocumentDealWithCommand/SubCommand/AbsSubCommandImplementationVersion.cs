using System;
using System.CommandLine;
using System.Collections.Generic;

using YTS.Log;

using DocumentDealWithCommand.Logic;
using DocumentDealWithCommand.Logic.Models;
using System.CommandLine.Invocation;

namespace DocumentDealWithCommand.SubCommand
{
    /// <summary>
    /// 子命令: 抽象基类实现
    /// </summary>
    public abstract class AbsSubCommandImplementationVersion<TParam> : AbsSubCommand, ISubCommand where TParam : BasicCommandParameters, new()
    {
        /// <summary>
        /// 实例化子命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="globalOptions">全局配置项</param>
        public AbsSubCommandImplementationVersion(ILog log, GlobalOptions globalOptions)
            : base(log, globalOptions) { }

        /// <summary>
        /// 命令名称标识
        /// </summary>
        public abstract string CommandNameSign();

        /// <summary>
        /// 命令描述
        /// </summary>
        public abstract string CommandDescription();

        /// <summary>
        /// 子命令配置
        /// </summary>
        public abstract IEnumerable<ISubCommand> SetSubCommands();

        /// <summary>
        /// 实现命令主函数方法
        /// </summary>
        public abstract IMain<TParam> HandlerLogic();

        /// <summary>
        /// 命令各选项配置注册项
        /// </summary>
        public abstract IEnumerable<Option> SetOptions();

        /// <summary>
        /// 写入参数内容
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="param">参数主题</param>
        /// <returns>返回参数</returns>
        public abstract TParam FillParam(InvocationContext context, TParam param);

        /// <inheritdoc/>
        public override Command GetCommand()
        {
            string name = CommandNameSign();
            string desc = CommandDescription();
            Command cmd = new Command(name, desc);
            IEnumerable<ISubCommand> subCommands = SetSubCommands();
            if (subCommands != null)
            {
                foreach (var item in subCommands)
                {
                    cmd.AddCommand(item.GetCommand());
                }
            }
            IEnumerable<Option> options = SetOptions();
            if (options != null)
            {
                foreach (var item in options)
                {
                    cmd.AddOption(item);
                }
            }
            IMain<TParam> main = HandlerLogic();
            if (main != null)
            {
                cmd.SetHandler((context) =>
                {
                    var logArgs = log.CreateArgDictionary();
                    try
                    {
                        var param = base.ToCommandParameters<TParam>(context);
                        param = FillParam(context, param);
                        param.WriteLogArgs(logArgs);
                        context.ExitCode = main.OnExecute(param);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"{desc} - 执行出错", ex, logArgs);
                        context.ExitCode = 1;
                    }
                });
            }
            return cmd;
        }
    }
}
