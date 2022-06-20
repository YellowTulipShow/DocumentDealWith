using System.Text;

using YTS.Log;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 主程序帮助类
    /// </summary>
    public class MainHelpr : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly ConfigHelper configHelper;

        /// <summary>
        /// 实例化 - 主程序帮助类
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public MainHelpr(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
            configHelper = new ConfigHelper(log, encoding);
        }

        /// <summary>
        /// 执行逻辑程序
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="cOption">用户传入的命令选项</param>
        public void OnExecute(string configFilePath, CommandOptions cOption)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = cOption;
            Configs configs = configHelper.ReadConfigs(configFilePath);
        }
    }
}
