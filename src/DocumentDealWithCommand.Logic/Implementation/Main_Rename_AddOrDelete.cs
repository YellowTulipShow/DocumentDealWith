using System;
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
        /// <inheritdoc/>
        public Main_Rename_AddOrDelete(ILog log) : base(log) { }

        /// <inheritdoc/>
        public override string ToResult(FileInfo data, int index)
        {
            MFileName mfile = FileInfoExtend.ToMFileName(data.Name);
            return ARFileName(mfile, param);
        }

        /// <summary>
        /// 增加删除文件名称各部分内容
        /// </summary>
        /// <param name="mfile">文件名称信息</param>
        /// <param name="param">替换参数</param>
        /// <returns>新名称</returns>
        public string ARFileName(MFileName mfile, ParamRenameAddOrDelete param)
        {
            string name = mfile.Name;
            string extension = mfile.Extension;

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
                if (!string.IsNullOrEmpty(param.UseExtendAddContent))
                {
                    uint index = ArrayDataExtend.UserInputToProgramTargetIndex(param.UseExtendAddStartCharIndex, name.Length + 1);
                    name = name.Insert((int)index, param.UseExtendAddContent);
                }
            }
            if (!string.IsNullOrEmpty(param.DeleteContent))
            {
                name = Regex.Replace(name, param.DeleteContent, string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.ECMAScript);
            }
            if (param.IsUseExtendDelete)
            {
                uint index = ArrayDataExtend.UserInputToProgramTargetIndex(param.UseExtendDeleteStartCharIndex, name.Length);
                uint count = Math.Max(0, param.UseExtendDeleteCount);
                count = Math.Min(count, (uint)name.Length - index);
                name = name.Remove((int)index, (int)count);
            }
            return $"{name}{extension}";
        }
    }
}
