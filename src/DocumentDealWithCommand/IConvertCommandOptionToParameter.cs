using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Text;

namespace DocumentDealWithCommand
{
    public interface IConvertCommandOptionToParameter
    {
        /// <summary>
        /// 获取需全局配置的命令行输入项
        /// </summary>
        IEnumerable<IInputOption> GetGlobalInputs();
        /// <summary>
        /// 获取命令行输入项
        /// </summary>
        IEnumerable<IInputOption> GetInputs();
    }

    public interface IConvertCommandOptionToParameter<TParam> : IConvertCommandOptionToParameter
    {
        IEnumerable<IInputOption> IConvertCommandOptionToParameter.GetGlobalInputs()
        {
            foreach (var item in GetInputs() ?? new IInputOption<TParam>[] { })
                if (item.IsGlobal())
                    yield return item;
        }
        IEnumerable<IInputOption> IConvertCommandOptionToParameter.GetInputs()
        {
            foreach (var item in GetInputs() ?? new IInputOption<TParam>[] { })
                if (!item.IsGlobal())
                    yield return item;
        }
        /// <summary>
        /// 获取命令行输入项
        /// </summary>
        new IEnumerable<IInputOption<TParam>> GetInputs();
    }
}
