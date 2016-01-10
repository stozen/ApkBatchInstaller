using System;
using System.Threading;
using NLog;

namespace ApkBatchInstaller
{
    class Program
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            string output;
            output = "开始运行...";
            ColorConsole.WriteLine(output, ConsoleColor.DarkGreen);
            logger.Info(output);

            ApkManager apk = new ApkManager();

            Thread apkInstall = new Thread(apk.Install);
            apkInstall.Start();

            while (true)
                Console.ReadKey();
        }
    }
}
