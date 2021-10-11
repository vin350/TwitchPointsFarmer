using System;
using System.Collections.Generic;
using TwitchPointsFarmer.Models;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TwitchPointsFarmer.Utils
{
    public static class SaveClass
    {
        public static string FolderPath { get; } = Environment.CurrentDirectory;
        public static string FilePath { get; } = Environment.CurrentDirectory + @"\config.json";
        public static JObject DefaultJson { get; } = new(
                    new JProperty("users", new JArray()),
                    new JProperty("channels", new JArray()));

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
                File.WriteAllText(FilePath, DefaultJson.ToString());
            }
        }

        public static void EnsureExists()
        {
            if (!DoesFileExists())
            {
                //nao existe
                Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FilePath, null);
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
                var x = new SaveContainer()
                {
                    MyChannels = channels,
                    MyUsers = users
                };
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(x));
                return true;
            }
            return false;
        }
        public static SaveContainer ReadFromFile()
        {
            EnsureExists();
            SaveContainer s = JsonConvert.DeserializeObject<SaveContainer>(File.ReadAllText(FilePath));
            return s;
        }
    }
}
