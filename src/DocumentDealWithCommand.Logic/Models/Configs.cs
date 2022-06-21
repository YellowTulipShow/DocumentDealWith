using Newtonsoft.Json;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class Configs
    {
        /// <summary>
        /// 全局操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 1)]
        public string[] Global_AllowExtensions { get; set; }

        /// <summary>
        /// content 命令操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 2)]
        public string[] ContentCommand_AllowExtensions { get; set; }

        /// <summary>
        /// rename 命令操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 3)]
        public string[] RenameCommand_AllowExtensions { get; set; }

        /// <summary>
        /// 获取默认的配置
        /// </summary>
        /// <returns>默认配置</returns>
        public static Configs GetDefaultConfigs()
        {
            return new Configs()
            {
                Global_AllowExtensions = new string[]
                {
                    // 文档
                    ".txt",
                    ".md",
                    ".mardown",
                    ".mardown",
                    ".gitignore",
                    ".drawio",
                },
                ContentCommand_AllowExtensions = new string[]
                {
                    // 语言
                    ".json",
                    ".cs",
                    ".csproj",
                    ".java",
                    ".c",
                    ".py",

                    // 脚本
                    ".ps1",
                    ".sh",
                    ".bat",
                },
                RenameCommand_AllowExtensions = new string[]
                {
                    // 图片
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".gif",
                    ".bmp",
                    ".svg",
                },
            };
        }
    }
}
