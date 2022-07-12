using System.Text.RegularExpressions;

using YTS.ConsolePrint;
using YTS.Log;

using DocumentDealWithCommand.Logic.Models;
using System.IO;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 替换
    /// </summary>
    public class Main_Rename_Replace : AbsMainReanme<ParamRenameReplace>
    {
        /// <inheritdoc/>
        public Main_Rename_Replace(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override string ToResult(FileInfo data, int index)
        {
            return RegexReplaceFileName(data.Name, param);
        }

        /// <summary>
        /// 正则替换文件名称
        /// </summary>
        /// <param name="fileName">文件名称(可带扩展名)</param>
        /// <param name="param">替换参数</param>
        /// <returns>新名称</returns>
        public string RegexReplaceFileName(string fileName, ParamRenameReplace param)
        {
            return Regex.Replace(fileName, param.Pattern, param.Replacement);
        }
    }
}
