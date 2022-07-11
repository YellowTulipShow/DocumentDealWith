using System.Collections.Generic;
using System.IO;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 处理重命名数据
    /// </summary>
    public interface IHandleRenameData : IHandleData<FileInfo, string>
    {
        /// <summary>
        /// 转为重命名结果方法
        /// </summary>
        /// <param name="datas">文件信息队列</param>
        /// <returns>重命名结果</returns>
        public IList<HandleRenameResult> ToReanmeResult(IList<FileInfo> datas)
        {
            IList<HandleRenameResult> rlist = new List<HandleRenameResult>();
            for (int i = 0; i < datas.Count; i++)
            {
                FileInfo data = datas[i];
                string result = ToResult(data, i);
                rlist.Add(new HandleRenameResult()
                {
                    Source = data,
                    Result = result,
                });
            }
            return rlist;
        }
    }
}
