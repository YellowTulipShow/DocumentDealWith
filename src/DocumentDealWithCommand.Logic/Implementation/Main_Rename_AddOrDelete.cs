using System.IO;
using System.Text.RegularExpressions;

using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 重命名 - 添加/删除
    /// </summary>
    public class Main_Rename_AddOrDelete : AbsMainReanme<ParamRenameAddOrDelete>
    {
        private readonly Regex nameRegex;
        /// <inheritdoc/>
        public Main_Rename_AddOrDelete(ILog log) : base(log)
        {
            nameRegex = new Regex(@"^(.*)\.([a-z0-9])$",
                RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
        }

        /// <inheritdoc/>
        public override string ToResult(FileInfo data, int index)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["nameRegex"] = nameRegex.ToString();
            logArgs["filePath"] = data.FullName;
            logArgs["index"] = index;

            string name = data.Name.Replace(data.Extension, string.Empty);
            string extension = data.Extension;

            if (!string.IsNullOrEmpty(param.BeforeAdd))
            {
                name = $"{param.BeforeAdd}{name}";
            }
            if (!string.IsNullOrEmpty(param.AfterAdd))
            {
                name = $"{name}{param.AfterAdd}";
            }
            if (param.IsUseExtendAdd)
            {
                if (!string.IsNullOrEmpty(param.UseExtendAddContent) &&
                    1 <= param.UseExtendAddStartCharIndex && param.UseExtendAddStartCharIndex <= name.Length)
                {
                    param.UseExtendAddStartCharIndex--;
                    name = name.Insert((int)param.UseExtendAddStartCharIndex, param.UseExtendAddContent);
                }
            }
            if (!string.IsNullOrEmpty(param.DeleteContent))
            {
                name = Regex.Replace(name, param.DeleteContent, string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            }
            if (param.IsUseExtendDelete)
            {
                if (1 <= param.UseExtendDeleteStartCharIndex && param.UseExtendDeleteStartCharIndex <= name.Length &&
                    1 <= param.UseExtendDeleteCount && param.UseExtendDeleteCount <= name.Length)
                {
                    param.UseExtendDeleteStartCharIndex--;
                    for (int i = 0; i < param.UseExtendDeleteCount; i++)
                    {
                        name = name.Remove((int)param.UseExtendDeleteStartCharIndex);
                    }
                }
            }
            return $"{name}{extension}";
        }
    }
}
