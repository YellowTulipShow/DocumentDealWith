using System.Collections.Generic;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.SubCommand;

using YTS.Log;

namespace DocumentDealWithCommand.Test.OverrideClass
{
    public interface ITestAllExecute :
        IExecute<CommandParameters_Content_Encoding>,
        IExecute<CommandParameters_Content_NewLine>,
        IExecute<ParamRenameWhole>,
        IExecute<ParamRenameAddOrDelete>,
        IExecute<ParamRenameReplace>
    {
    }

    /// <inheritdoc/>
    public class Test_MainCommand : MainCommand
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_MainCommand(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Content(log, exe);
            yield return new Test_SubCommand_Rename_Whole(log, exe);
        }
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Content : SubCommand_Content
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Content(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Content_Encoding(log, exe);
            yield return new Test_SubCommand_Content_NewLine(log, exe);
        }
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Content_Encoding : SubCommand_Content_Encoding
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Content_Encoding(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IExecute<CommandParameters_Content_Encoding> GetExecute() => exe;
    }
    /// <inheritdoc/>
    public class Test_SubCommand_Content_NewLine : SubCommand_Content_NewLine
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Content_NewLine(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IExecute<CommandParameters_Content_NewLine> GetExecute() => exe;
    }


    /// <inheritdoc/>
    public class Test_SubCommand_Rename_Whole : SubCommand_Rename_Whole
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Rename_Whole(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute() => exe;

        /// <inheritdoc/>
        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Rename_Replace(log, exe);
            yield return new Test_SubCommand_Rename_AddOrDelete(log, exe);
        }
    }
    /// <inheritdoc/>
    public class Test_SubCommand_Rename_AddOrDelete : SubCommand_Rename_AddOrDelete
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Rename_AddOrDelete(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IExecute<ParamRenameAddOrDelete> GetExecute() => exe;
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Rename_Replace : SubCommand_Rename_Replace
    {
        private readonly ITestAllExecute exe;
        /// <inheritdoc/>
        public Test_SubCommand_Rename_Replace(ILog log, ITestAllExecute exe) : base(log)
        {
            this.exe = exe;
        }

        public override IExecute<ParamRenameReplace> GetExecute() => exe;
    }
}
