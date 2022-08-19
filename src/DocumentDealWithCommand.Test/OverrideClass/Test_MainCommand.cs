using System.Collections.Generic;

using CommandParamUse;

using DocumentDealWithCommand.Logic.Models;
using DocumentDealWithCommand.SubCommand;

using YTS.Log;

namespace DocumentDealWithCommand.Test.OverrideClass
{



    /// <inheritdoc/>
    public class Test_MainCommand : MainCommand
    {
        /// <inheritdoc/>
        public Test_MainCommand(ILog log) : base(log) { }

        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Content(log);
            yield return new Test_SubCommand_Rename_Whole(log);
        }
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Content : SubCommand_Content
    {
        /// <inheritdoc/>
        public Test_SubCommand_Content(ILog log) : base(log) { }

        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Content_Encoding(log);
            yield return new Test_SubCommand_Content_NewLine(log);
        }
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Content_Encoding : SubCommand_Content_Encoding
    {
        /// <inheritdoc/>
        public Test_SubCommand_Content_Encoding(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute()
        {
        }
    }
    /// <inheritdoc/>
    public class Test_SubCommand_Content_NewLine : SubCommand_Content_NewLine
    {
        /// <inheritdoc/>
        public Test_SubCommand_Content_NewLine(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute()
        {
        }
    }


    /// <inheritdoc/>
    public class Test_SubCommand_Rename_Whole : SubCommand_Rename_Whole
    {
        /// <inheritdoc/>
        public Test_SubCommand_Rename_Whole(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute()
        {
        }

        /// <inheritdoc/>
        public override IEnumerable<ICommandSub> GetSubCommands()
        {
            yield return new Test_SubCommand_Rename_Replace(log);
            yield return new Test_SubCommand_Rename_AddOrDelete(log);
        }
    }
    /// <inheritdoc/>
    public class Test_SubCommand_Rename_AddOrDelete : SubCommand_Rename_AddOrDelete
    {
        /// <inheritdoc/>
        public Test_SubCommand_Rename_AddOrDelete(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute()
        {
        }
    }

    /// <inheritdoc/>
    public class Test_SubCommand_Rename_Replace : SubCommand_Rename_Replace
    {
        /// <inheritdoc/>
        public Test_SubCommand_Rename_Replace(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override IExecute<ParamRenameWhole> GetExecute()
        {
        }
    }
}
