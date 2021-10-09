using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchPointsFarmer.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TwitchPointsFarmer
{
    public static class SaveClass
    {
        public static string FolderPath { get; } = Environment.CurrentDirectory;
        public static string FilePath { get; } = Environment.CurrentDirectory + @"\config.json";

        public static bool DoesFileExists()
        {
            if (Directory.Exists(FolderPath))
            {
                if (File.Exists(FilePath))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsFileEmpty()
        {
            if (DoesFileExists())
            {
                string file = File.ReadAllText(FilePath);
                if (string.IsNullOrWhiteSpace(file))
                {
                    return true;
                }
            }
            return false;
        }

        public static void WriteDefaultFile()
        {
            if (DoesFileExists())
            {
                JObject defaultJson = new(
                    new JProperty("users", new JArray()),
                    new JProperty("channels", new JArray()));
                File.WriteAllText(FilePath, defaultJson.ToString());
            }
        }

        public static void EnsureExists()
        {
            if (!DoesFileExists())
            {
                //nao existe
                Directory.CreateDirectory(FolderPath);
                File.Create(FilePath);
                WriteDefaultFile();
            }
            else
            {
                //arquivo existe
                if (IsFileEmpty())
                {
                    WriteDefaultFile();
                }
            }
        }

        public static bool SaveToFile(List<User> users, List<string> channels)
        {
            EnsureExists();
            if(users!=null && channels != null)
            {
                
                return true;
            }
            return false;
        }
        public static (List<User>,List<string>) ReadFromFile()
        {
            EnsureExists();
            JObject json = JObject.Parse(FilePath);
            var users = json["users"].ToObject<List<User>>();
            var channels = json["channels"].ToObject<List<string>>();
            return (users, channels);
        }
    }
}
