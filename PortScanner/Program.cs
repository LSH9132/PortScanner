using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


";
        static void Main(string[] args)
        {
            Logger.info("Hello");
            Console.WriteLine(lshworkspace);
            Console.Write("Please input ipaddress : ");
            Console.ReadLine();
        }
    }
}
