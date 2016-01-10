using System;

namespace ApkBatchInstaller
{
    class ColorConsole
    {
        public static void WriteLine(string value, ConsoleColor color)
        {
            value = "[" + DateTime.Now.ToLongTimeString() + "] " + value;

            Console.ForegroundColor = color;

            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));

            Console.ResetColor();
        }
    }
}
