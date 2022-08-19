using System.IO;
using System.Collections.Generic;

using YTS.Log;

using CommandParamUse;
using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <inheritdoc/>
    public abstract class AbsMainReanme<T> : AbsUseLog, IExecute<T>, IHandleRenameData where T : AbsParamRename, new()
    {
        /// <summary>
        /// 更改文件名称执行方法
        /// </summary>
        protected readonly ChangeFileNameHelp changeHelp;
        /// <summary>
        /// 执行参数
        /// </summary>
        protected T param;

        /// <inheritdoc/>
        public AbsMainReanme(ILog log) : base(log)
        {
            changeHelp = new ChangeFileNameHelp(log);
        }

        /// <inheritdoc/>
        public virtual int OnExecute(T param)
        {
            SetParam(param);
            var hand = (IHandleRenameData)this;
            IList<HandleRenameResult> rlist;
            if (param.IsPreview)
            {
                var control = new RenamePreviewProcessControl(log, param.Print, hand, param.PreviewColumnCount, param.ConsoleType);
                rlist = control.OnExecutePreview(param.NeedHandleFileInventory);
            }
            else
            {
                rlist = hand.ToReanmeResult(param.NeedHandleFileInventory);
            }
            if (rlist == null || rlist.Count <= 0)
            {
                param.Print.WriteLine($"可操作文件列表为空, 已终止!");
                return 0;
            }
            return changeHelp.ChangeFileName(param.Print, param.RootDire, rlist);
        }

        /// <summary>
        /// 赋值参数内容
        /// </summary>
        /// <param name="param">参数</param>
        public void SetParam(T param)
        {
            this.param = param;
        }

        /// <inheritdoc/>
        public abstract string ToResult(FileInfo data, int index);

        /// <inheritdoc/>
        public string ToPrint(FileInfo data)
        {
            return data.ToShowFileName(param.RootDire);
        }
    }
}
