using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using WerhausManager;
using System.Windows;

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
        public static void showErrorMessage(Exception ex)
        {
            if (ex is WebserviceError)
            {
                WebserviceError we = ex as WebserviceError;
                MessageBox.Show(we.messageDetails + "\n", we.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show(ex.Message + "\n", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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

