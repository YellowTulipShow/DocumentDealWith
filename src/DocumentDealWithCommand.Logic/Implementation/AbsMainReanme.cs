using System.IO;
using System.Collections.Generic;

using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <inheritdoc/>
    public abstract class AbsMainReanme<T> : AbsMain, IMain<T>, IHandleRenameData where T : AbsParamRename, new()
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
            this.param = param;
            var hand = (IHandleRenameData)this;
            IList<HandleRenameResult> rlist;
            if (param.IsPreview)
            {
                var control = new RenamePreviewProcessControl(log, param.Print, hand);
                rlist = control.OnExecutePreview(param.NeedHandleFileInventory);
            }
            else
            {
                rlist = hand.ToReanmeResult(param.NeedHandleFileInventory);
            }
            return changeHelp.ChangeFileName(param.Print, param.RootDire, rlist);
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
