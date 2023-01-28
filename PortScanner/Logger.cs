﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    static class Logger
    {
        private static string getDateTime()
        {
            DateTime dateTime = DateTime.Now.ToLocalTime();
            string time = dateTime.ToString("HH:mm:ss");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{time} ");
            return time;
        }

        public static void info(string msg)
        {
            getDateTime();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("[ INFO ] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(msg);
        }
    }
}
