using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using DocumentDealWithCommand.Logic.Models;

using YTS.Log;
using YTS.ConsolePrint;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 实现命令类: 内容操作 - 编码修改
    /// </summary>
    public class Main_Content_Encoding : AbsMain, IMain<CommandParameters_Content_Encoding>
    {
        /// <inheritdoc/>
        public Main_Content_Encoding(ILog log, IPrintColor print) : base(log, print)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <inheritdoc/>
        public int OnExecute(CommandParameters_Content_Encoding commandParameters)
        {
            log.Info($"转换编码, 参数 - Encoding: {commandParameters.Encoding}");

            var rinventory = commandParameters.NeedHandleFileInventory;
            foreach (var item in rinventory)
            {
                print.WriteLine($"文件路径: {item.FullName}");
                HandleFile(item, commandParameters.Encoding);
            }
            return 0;
        }

        private void HandleFile(FileInfo file, ECodeType codeType)
        {
            //string content = File.ReadAllText(file.FullName);
            //File.WriteAllText(file.FullName, content, Encoding.UTF8);
            var path = file.FullName;
            //string content = File.ReadAllText(path, Encoding.ASCII);
            //print.WriteLine(content);

            //byte[] data = File.ReadAllBytes(path);
            //foreach (var item in data)
            //{
            //    print.WriteLine(BitConverter.ToString(item));
            //}
            // 注册编码

            //foreach (EncodingInfo eInfo in Encoding.GetEncodings())
            //{
            //    Console.WriteLine("Encoding code page is {0}, encoding name is {1}", eInfo.CodePage, eInfo.Name);
            //    Console.WriteLine("Encoding dispaly name is {0}", eInfo.DisplayName);
            //}

            //Encoding fileCodeFormat = GetFileEncodeType(path);
            //print.WriteLine($"fileCodeFormat.BodyName: {fileCodeFormat.BodyName}");
            //print.WriteLine($"fileCodeFormat.HeaderName: {fileCodeFormat.HeaderName}");
            //print.WriteLine($"fileCodeFormat.WebName: {fileCodeFormat.WebName}");
            //print.WriteLine($"fileCodeFormat.EncodingName: {fileCodeFormat.EncodingName}");

            //Encoding gb2312 = Encoding.GetEncoding("GB2312");
            //print.WriteLine($"gb2312.BodyName: {gb2312.BodyName}");
            //print.WriteLine($"gb2312.HeaderName: {gb2312.HeaderName}");
            //print.WriteLine($"gb2312.WebName: {gb2312.WebName}");
            //print.WriteLine($"gb2312.EncodingName: {gb2312.EncodingName}");

        }

        public Encoding GetFileEncodeType(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using BinaryReader br = new BinaryReader(fs);
                byte[] buffer = br.ReadBytes(2);
                if (buffer[0] >= 0xEF)
                {
                    if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                    {
                        return System.Text.Encoding.UTF8;
                    }
                    else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                    {
                        return System.Text.Encoding.BigEndianUnicode;
                    }
                    else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                    {
                        return System.Text.Encoding.Unicode;
                    }
                    else
                    {
                        return System.Text.Encoding.Default;
                    }
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            };
        }
    }


    public class ANSI_Encoding : EncodingProvider
    {
        public override Encoding GetEncoding(int codepage)
        {
            throw new NotImplementedException();
        }

        public override Encoding GetEncoding(string name)
        {
            throw new NotImplementedException();
        }
    }
}

/// <summary> 
/// 获取文件的编码格式 
/// </summary> 
public class EncodingType
{
    /// <summary> 
    /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型 
    /// </summary> 
    /// <param name=“FILE_NAME“>文件路径</param> 
    /// <returns>文件的编码类型</returns> 
    public static System.Text.Encoding GetType(string FILE_NAME, List<string> ResultList)
    {
        FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
        Encoding r = GetType(fs, FILE_NAME, ResultList);
        fs.Close();
        return r;
    }

    /// <summary> 
    /// 通过给定的文件流，判断文件的编码类型 
    /// </summary> 
    /// <param name=“fs“>文件流</param> 
    /// <returns>文件的编码类型</returns> 
    public static System.Text.Encoding GetType(FileStream fs, string FILE_NAME, List<string> ResultList)
    {
        //byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        //byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        //byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM 
        Encoding reVal = Encoding.Default;
        BinaryReader r = new BinaryReader(fs, System.Text.Encoding.Default);
        int i;
        int.TryParse(fs.Length.ToString(), out i);
        byte[] ss = r.ReadBytes(i);
        if (IsUTF8Bytes(ss, FILE_NAME, ResultList) || (ss.Length > 3 && ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
        {
            reVal = Encoding.UTF8;
        }
        else if (ss.Length > 3 && ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
        {
            reVal = Encoding.BigEndianUnicode;
        }
        else if (ss.Length > 3 && ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
        {
            reVal = Encoding.Unicode;
        }
        r.Close();
        return reVal;

    }

    /// <summary> 
    /// 判断是否是不带 BOM 的 UTF8 格式 
    /// </summary> 
    /// <param name=“data“></param> 
    /// <returns></returns> 
    private static bool IsUTF8Bytes(byte[] data, string FILE_NAME, List<string> ResultList)
    {
        int charByteCounter = 1; //计算当前正分析的字符应还有的字节数 
        byte curByte; //当前分析的字节. 
        for (int i = 0; i < data.Length; i++)
        {
            curByte = data[i];
            if (charByteCounter == 1)
            {
                if (curByte >= 0x80)
                {
                    //判断当前 
                    while (((curByte <<= 1) & 0x80) != 0)
                    {
                        charByteCounter++;
                    }
                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X 
                    if (charByteCounter == 1 || charByteCounter > 6)
                    {
                        return false;
                    }
                }
            }
            else
            {
                //若是UTF-8 此时第一位必须为1 
                if ((curByte & 0xC0) != 0x80)
                {
                    return false;
                }
                charByteCounter--;
            }
        }
        if (charByteCounter > 1)
        {
            ResultList.Add($"{FILE_NAME}，异常：非预期的byte格式，无法判断是否是UTF8(不带BOM)格式,已跳过");
            //throw new Exception("非预期的byte格式");
        }
        return true;
    }

}