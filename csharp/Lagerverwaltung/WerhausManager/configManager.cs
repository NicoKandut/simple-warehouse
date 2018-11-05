using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
namespace Lagerverwaltung
{
    public static class configManager
    {
        public static string Path { private get; set; }
        public static void init(string configPath)
        {
            Path = configPath;
        }
        public static void getUserFromConfig()
        {
            
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
