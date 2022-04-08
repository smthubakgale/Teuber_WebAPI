using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Web.Deploy;
using Web.Deploy.Views.Layout;
using WebAPIsameDomain.Models;

namespace Desktop.Database.offline
{
    public class Logger
    {
        private static string dir = @"C:\Teuber\Cookies\log";
        private string path { get; set; }
        private static string file = @"\log.txt";
        public Logger()
        {
            if (!(new JC<int>()).isfinal) 
            { 
                // Directory 
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                path = dir + file;
                // File 
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("");
                    }
                }
                // 
            }
        }
        public void Clear(string cl)
        {
            if (!(new JC<int>()).isfinal)
            {
                File.WriteAllText(path, string.Empty);
            }
            else
            {
                new JC<int>().Clear(cl);
            }
        }
        public void Log(string cl , string msg)
        {
            if (!(new JC<int>()).isfinal)
            {

                using (StreamWriter w = File.AppendText(path))
                {
                    Logs(msg, w);
                }

                using (StreamReader r = File.OpenText(path))
                {
                    DumpLog(r);
                }
            }
            else
            {
                new JC<int>().Log(cl, msg);
            }
        }

        public static void Logs(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
