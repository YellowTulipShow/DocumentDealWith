using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;

using YTS.Log;

namespace CommandParamUse
{
    public abstract class AbsCommand
    {
        private readonly ILog log;
        private readonly ICommand parent;
        public AbsCommand(ILog log, ICommand parent)
        {
        }
    }
}
