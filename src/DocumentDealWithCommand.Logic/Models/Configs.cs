using Newtonsoft.Json;

namespace DocumentDealWithCommand.Logic.Models
{
    /// <summary>
    /// 配置项
    /// </summary>
    public class Configs
    {
        /// <summary>
        /// 允许扩展名配置
        /// </summary>
        [JsonProperty(Order = 1)]
        public Configs_AllowExtension AllowExtension { get; set; }

        /// <summary>
        /// 获取默认的配置
        /// </summary>
        /// <returns>默认配置</returns>
        public static Configs GetDefaultConfigs()
        {
            return new Configs()
            {
                AllowExtension = new Configs_AllowExtension()
                {
                    Global = new string[]
                    {
                        // 文档
                        ".txt",
                        ".md",
                        ".mardown",
                        ".mardown",
                        ".gitignore",
                        ".drawio",
                    },
                    ContentCommand = new string[]
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
                    RenameCommand = new string[]
                    {
                        // 图片
                        ".jpg",
                        ".jpeg",
                        ".png",
                        ".gif",
                        ".bmp",
                        ".svg",
                    },
                },
            };
        }
    }

    /// <summary>
    /// 配置项: 允许扩展名配置
    /// </summary>
    public struct Configs_AllowExtension
    {
        /// <summary>
        /// 全局操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 1)]
        public string[] Global { get; set; }

        /// <summary>
        /// content 命令操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 2)]
        public string[] ContentCommand { get; set; }

        /// <summary>
        /// rename 命令操作文件: 允许扩展名
        /// </summary>
        [JsonProperty(Order = 3)]
        public string[] RenameCommand { get; set; }
    }
}
