using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace PortScanner
{
    class Program
    {
        static string lshworkspace = @"
   __  __                             _                             
  / / / _\  /\  /\__      _____  _ __| | _____ _ __   __ _  ___ ___ 
 / /  \ \  / /_/ /\ \ /\ / / _ \| '__| |/ / __| '_ \ / _` |/ __/ _ \
/ /____\ \/ __  /  \ V  V / (_) | |  |   <\__ \ |_) | (_| | (_|  __/
\____/\__/\/ /_/    \_/\_/ \___/|_|  |_|\_\___/ .__/ \__,_|\___\___|
                                              |_|                   
  _____           _    _____                                 
 |  __ \         | |  / ____|                                
 | |__) |__  _ __| |_| (___   ___ __ _ _ __  _ __   ___ _ __ 
 |  ___/ _ \| '__| __|\___ \ / __/ _` | '_ \| '_ \ / _ \ '__|
 | |  | (_) | |  | |_ ____) | (_| (_| | | | | | | |  __/ |   
 |_|   \___/|_|   \__|_____/ \___\__,_|_| |_|_| |_|\___|_|   
";
        static IPAddress Ipaddress;
        static uint ThreadCounter;
        static int minPort;
        static int maxPort;
        static string savePath;
        static int totalFound = 0;

        private static int PortCounter = -1;

        static void Main(string[] args)
        {
            Console.Title = "Happy Hacking :D";
            Console.WriteLine(lshworkspace);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Github : https://github.com/LSH9132/PortScanner.git");
            Console.WriteLine("Website : https://lshworkspace.com/");
            Console.ForegroundColor = ConsoleColor.Gray;

            while(true)
            {
                Console.Write("\nPlease input ipaddress : ");
                try
                {
                    string ip = Console.ReadLine();
                    Ipaddress = IPAddress.Parse(ip);
                    break;
                }
                catch
                {
                    Logger.error("Invalid format... Please retry. :(");
                    continue;
                }
            }
            Console.WriteLine("");
            Logger.info("Port Minimum : 0 Maximum : 65535");
            while (true)
            {
                Console.Write("\nPlease input min port : ");
                try
                {
                    minPort = Int32.Parse(Console.ReadLine());
                    if(minPort < 0 || minPort > 65535)
                    {
                        Logger.error("You only have to choose between 0 and 65535.. Please retry. :(");
                        continue;
                    }
                    break;
                }
                catch
                {
                    Logger.error("Invalid format... Please retry. :(");
                    continue;
                }
            }

            if(minPort != 65535)
            {
                while (true)
                {
                    Console.Write("\nPlease input max port : ");
                    try
                    {
                        maxPort = Int32.Parse(Console.ReadLine());
                        if (maxPort > 65535)
                        {
                            Logger.error("Only numbers less than 65535 must be selected. Please retry. :(");
                            continue;
                        }
                        if (maxPort < minPort)
                        {
                            Logger.error($"You cannot select a number smaller than the min port..\nYour min port is {minPort}. Please retry. :(");
                        }
                        break;
                    }
                    catch
                    {
                        Logger.error("Invalid format... Please retry. :(");
                        continue;
                    }
                }
            }
            else
            {
                maxPort = 65535;
            }

            while (true)
            {
                Console.Write("\nPlease input using thread count (recommend 100) : ");
                try
                {
                    uint count = UInt32.Parse(Console.ReadLine());
                    if(count >= 1)
                    {
                        ThreadCounter = count;
                        break;
                    }
                    continue;
                }
                catch
                {
                    Logger.error("Invalid format... Please retry. :(");
                    continue;
                }
            }
            while(true)
            {
                Console.WriteLine("\nSelected IPaddress : {0}\nSelected Scan from {1} to {2}\n minSelected Thread count : {3}\n", Ipaddress, minPort, maxPort, ThreadCounter);
                Console.Write("Are you sure want to start? (Y/n) ");
                try
                {
                    string sure = Console.ReadLine();
                    if(sure == "Y")
                    {
                        break;
                    }
                    else if (sure == "n")
                    {
                        return;
                    }
                    continue;
                }
                catch { continue; }
            }
            PortCounter = (minPort - 1);

            string dataPath = (AppDomain.CurrentDomain.BaseDirectory + @"\ScannerData");

            if(!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            int fileI = 0;
            while (true)
            {
                if (fileI <= 0)
                {
                    savePath = (dataPath + @"\" + Ipaddress.ToString() + ".log");
                }
                else
                {
                    savePath = (dataPath + @"\" + Ipaddress.ToString() + @" (" + fileI + @")" + ".log");
                }

                if (!File.Exists(savePath))
                {
                    break;
                }

                fileI++;
                continue;
            }

            Program program = new Program();
            Thread[] threadArray = new Thread[ThreadCounter];
            for(int i = 0; i <= ThreadCounter - 1; i++)
            {
                threadArray[i] = new Thread(() => program.threader());
                threadArray[i].IsBackground = true;
                threadArray[i].Start();
                continue;
            }

            for (int i = 0; i <= ThreadCounter - 1; i++)
            {
                try
                {
                    threadArray[i].Join();
                }
                catch { continue; }
            }
            Console.Title = $"DONE :D | Total founded : {totalFound}";
            Logger.info("DONE.");
            try
            {
                Process.Start(savePath);
            }
            catch { }
            Console.ReadLine();
        }

        private object lockObject = new object();

        private int CountSafer()
        {
            lock (lockObject)
            {
                if(PortCounter >= maxPort)
                {
                    return -1;
                }
                PortCounter++;
                Console.Title = $"Now scanning port : {PortCounter} | Total founded : {totalFound}";
                return PortCounter;
            }
        }

        private void threader()
        {
            while (true)
            {
                int port = CountSafer();
                if (port <= -1)
                {
                    break;
                }
                if (ScanPort(port))
                {
                    if(totalFound == 0)
                    {
                        File.AppendAllText(savePath, $"{port}");
                    }
                    else
                    {
                        File.AppendAllText(savePath, $"\r{port}");
                    }
                    totalFound++;
                    Logger.info($"{port} is opened.");
                }
                continue;
            }
        }

        private bool ScanPort(int port)
        {
            try
            {
                IPEndPoint pEndPoint = new IPEndPoint(Ipaddress, port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(pEndPoint);
                return true;
            } catch { return false; }
        }
    }
}
