using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Text;

namespace CommandParamUse
{
    public abstract class AbsExecute<TParam> : IExecute<TParam> where TParam : IParameters
    {
        IEnumerable<IOption> IExecute.GetGlobalOptions()
        {
            foreach (var item in GetOptions() ?? new IOption<TParam>[] { })
                if (item.IsGlobal())
                    yield return item;
        }
        IEnumerable<IOption> IExecute.GetOptions()
        {
            foreach (var item in GetOptions() ?? new IOption<TParam>[] { })
                if (!item.IsGlobal())
                    yield return item;
        }
        public virtual int OnExecute(InvocationContext context)
        {
            TParam param = GetParam();
            foreach (var item in GetOptions() ?? new IOption<TParam>[] { })
            {
                item.FillParam(context, param);
            }
            return OnExecute(context, param);
        }

        public abstract IEnumerable<IOption<TParam>> GetOptions();
        public abstract bool IsExecute();
        public abstract TParam GetParam();
        public abstract int OnExecute(InvocationContext context, TParam param);
    }
}
