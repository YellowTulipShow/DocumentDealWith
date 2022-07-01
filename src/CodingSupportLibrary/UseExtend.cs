﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodingSupportLibrary
{
    /// <summary>
    /// 静态调用扩展方法
    /// </summary>
    public static class UseExtend
    {
        /// <summary>
        /// 注册支持代码页
        /// </summary>
        public static void SupportCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 判断得到文件的编码内容
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <returns>编码结果, 如果不受支持无法找到则返回 NULL</returns>
        public static Encoding GetEncoding(this FileInfo file)
        {
            JudgeEncodingResponse response = file.GetJudgeEncodingResponse();
            return response.Encoding?.ToEncoding();
        }

        /// <summary>
        /// 判断得到文件的编码内容判断结果响应
        /// </summary>
        /// <param name="file">文件内容</param>
        /// <returns>编码内容判断结果响应</returns>
        public static JudgeEncodingResponse GetJudgeEncodingResponse(this FileInfo file)
        {
            JudgeEncodingResponse response = GetDefaultResponse();
            byte[] contentBytes = null;
            foreach (var item in GetJudgeEncodings())
            {
                bool isNullContentBytes = contentBytes == null || contentBytes.Length <= 0;
                response = isNullContentBytes ?
                    item.GetEncoding(file) :
                    item.GetEncoding(contentBytes);
                if (response.Encoding != null)
                    return response;
                if (response.IsReadFileALLContent)
                    contentBytes = response.ContentBytes;
            }
            return response;
        }

        /// <summary>
        /// 获取默认响应
        /// </summary>
        /// <returns>默认响应</returns>
        internal static JudgeEncodingResponse GetDefaultResponse()
        {
            return new JudgeEncodingResponse()
            {
                IsReadFileALLContent = false,
                ContentBytes = null,
                Encoding = null,
            };
        }

        private static IEnumerable<IJudgeEncoding> GetJudgeEncodings()
        {
            yield return new JudgeEncoding.UnicodeHeader();
            yield return new JudgeEncoding.ASCII();
            yield return new JudgeEncoding.UTF8NoBOM();
            yield return new JudgeEncoding.Chinese();
        }

        /// <summary>
        /// 疑似受支持编码空枚举转为编码抽象类
        /// </summary>
        /// <param name="enum_support_encoding">疑似受支持编码空枚举</param>
        /// <returns>编码抽象类</returns>
        public static Encoding ToEncoding(this ESupportEncoding? enum_support_encoding)
        {
            if (enum_support_encoding == null)
                throw new ArgumentNullException(nameof(enum_support_encoding), $"编码配置枚举值为空, 无法解析!");
            return enum_support_encoding?.ToEncoding();
        }
        /// <summary>
        /// 受支持编码枚举转为编码抽象类
        /// </summary>
        /// <param name="enum_support_encoding">受支持编码枚举</param>
        /// <returns>编码抽象类</returns>
        public static Encoding ToEncoding(this ESupportEncoding enum_support_encoding)
        {
            return enum_support_encoding switch
            {
                ESupportEncoding.ASCII => Encoding.ASCII,
                ESupportEncoding.UTF32_LittleEndian => Encoding.UTF32,
                ESupportEncoding.UTF16_LittleEndian => Encoding.Unicode,
                ESupportEncoding.UTF16_BigEndian => Encoding.BigEndianUnicode,
                ESupportEncoding.UTF8 => new UTF8Encoding(true),
                ESupportEncoding.UTF8_NoBOM => new UTF8Encoding(false),
                ESupportEncoding.GBK => Encoding.GetEncoding("GBK"),
                _ => throw new ArgumentOutOfRangeException(nameof(enum_support_encoding), $"受支持的编码配置, 无法解析: {enum_support_encoding}"),
            };
        }
    }
}
