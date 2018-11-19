using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using WerhausManager;

namespace Lagerverwaltung
{
    public static class configManager
    {
        public static string Path { private get; set; }
        public static void init()
        {
            Path = @"./log.txt";
        }
        public static void getUserFromConfig()
        {
            
        }
        public static void writeLog(string information, LogType type)
        {
            CheckForLogFileCreateIfMissing();
            using (StreamWriter sw = new StreamWriter(Path))
            {
                sw.WriteLine("[" + DateTime.Now + "]" + "[" + type.ToString() + "]" + information);
            }           
        }
        private static void CheckForLogFileCreateIfMissing()
        {
            if (!File.Exists(Path))            
                File.Create(Path);
        }
    }
}
public class User
{
    public string Name { get; set; }
    public string Password { get; set; }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }
}

