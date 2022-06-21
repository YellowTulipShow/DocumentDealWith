using System;
using System.Text;

using YTS.Log;

namespace DocumentDealWithCommand
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var logFile = ILogExtend.GetLogFilePath("Program");
                ILog log = new FilePrintLog(logFile, Encoding.UTF8)
                    .Connect(new ConsolePrintLog());
                CommandArgsParser commandArgsParser = new CommandArgsParser(log);
                return commandArgsParser.OnParser(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序出错: {ex.Message}");
                Console.WriteLine($"堆栈信息: {ex.StackTrace ?? string.Empty}");
                return 1;
            }
        }
    }
}
