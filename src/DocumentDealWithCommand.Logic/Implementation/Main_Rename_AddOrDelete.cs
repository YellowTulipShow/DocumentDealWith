using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;
using System.IO;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 添加/删除
    /// </summary>
    public class Main_Rename_AddOrDelete : AbsMainReanme<ParamRenameAddOrDelete>
    {
        /// <inheritdoc/>
        public Main_Rename_AddOrDelete(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override string ToResult(FileInfo data, int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
