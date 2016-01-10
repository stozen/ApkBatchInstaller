using System;
using System.Collections.Generic;
using NLog;
using JsonConfig;
using System.Threading;

namespace ApkBatchInstaller
{
    class ApkManager
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 启动状态
        /// </summary>
        private bool run = false;

        /// <summary>
        /// 设备日志存储路径
        /// </summary>
        public string LogDir { get; private set; }

        /// <summary>
        /// PC本地预安装的APK文件路径
        /// </summary>
        public string ApkFilePath { get; private set; }

        /// <summary>
        /// APK的包名称(命名空间)
        /// </summary>
        public string ApkPackageName { get; private set; }

        /// <summary>
        /// APK引导Activity的类名
        /// </summary>
        public string ApkBootActivityClass { get; private set; }

        /// <summary>
        /// 命令行集
        /// </summary>
        public IList<SingleCmd> Commands { get; private set; }

        /// <summary>
        /// 设备集
        /// </summary>
        public IDictionary<string, Device> Devices { get; private set; }

        /// <summary>
        /// 进行安装
        /// </summary>
        public void Install()
        {
            string output;

            try
            {
                if (!run)
                {
                    run = true;

                    output = "开始安装...";
                    ColorConsole.WriteLine(output, ConsoleColor.DarkGreen);
                    logger.Info(output);

                    InitConfig();

                    IList<Thread> subThreads = new List<Thread>();
                    foreach (KeyValuePair<string, Device> kvp in Devices)
                    {
                        Thread subThread = new Thread(new ParameterizedThreadStart(this.SingleInstall));
                        subThread.Start(kvp.Key);
                    }

                    /*
                    output = "安装完毕!";
                    ColorConsole.WriteLine(output, ConsoleColor.DarkGreen);
                    logger.Info(output);
                     * */
                    run = false;
                }
            }
            catch (Exception ex)
            {
                output = "错误! " + ex.Message + ex.InnerException;
                ColorConsole.WriteLine(output, ConsoleColor.DarkRed);
                logger.Error(output);
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitConfig()
        {
            try
            {
                LogDir = Config.User.LogDir;
                ApkFilePath = Config.User.ApkFilePath;
                ApkPackageName = Config.User.ApkPackageName;
                ApkBootActivityClass = Config.User.ApkBootActivityClass;
                Commands = new List<SingleCmd>();
                foreach (dynamic cmd in Config.User.Cmds)
                {
                    string id = cmd.ID;
                    string name = cmd.Name;
                    string line = cmd.Cmd.Replace("@pn", ApkPackageName);
                    line = line.Replace("@apk", ApkFilePath);
                    line = line.Replace("@cls", ApkBootActivityClass);
                    Commands.Add(new SingleCmd(id, name, line));
                }
                Devices = new Dictionary<string, Device>();
                foreach (dynamic device in Config.User.Devices)
                {
                    string id = device.ID;
                    string name = device.Name;
                    string mac = device.Mac;
                    Devices.Add(id, new Device(id, name, mac));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("初始化配置错误", ex);
            }
        }

        /// <summary>
        /// 安装一台设备
        /// </summary>
        /// <param name="deviceID"></param>
        private void SingleInstall(object deviceID)
        {
            try
            {
                Device device = Devices[deviceID.ToString()];
                foreach (SingleCmd cmd in Commands)
                {
                    string line = cmd.Cmd.Replace("@id", device.ID);
                    string logFile = LogDir + "/" + device.Name + ".device.log";
                    Log.Write(">>>>>>>>> " + cmd.ID + ": " + cmd.Name + " >>>>>>>>>", logFile); ;
                    Cmd.Execute(line, logFile);
                    Log.Write("<<<<<<<<< " + cmd.ID + ": " + cmd.Name + " <<<<<<<<<", logFile);
                }
            }
            catch (Exception ex)
            {
                logger.Error("安装设备" + deviceID + "错误 " + ex.Message + ex.InnerException);
            }
        }
    }
}
