using System;
using System.IO;

namespace ApkBatchInstaller
{
    class Log
    {
        public static void Write(string message, string fileName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
                sw.WriteLine("[" + DateTime.Now.ToString() + "]" + " " + message);
                sw.Close();
            }
            catch (Exception ex) { Write(ex.Message, fileName); }
        }
    }
}
