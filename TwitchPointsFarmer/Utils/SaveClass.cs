using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TwitchPointsFarmer.Models;

namespace TwitchPointsFarmer.Utils
{
    public class SaveClass
    {
        public string FolderPath { get; } = Environment.CurrentDirectory;
        public string FilePath { get; } = Environment.CurrentDirectory + @"\config.json";
        private JObject DefaultJson { get; } = new(
                    new JProperty("users", new JArray()),
                    new JProperty("channels", new JArray()));

        private bool DoesFileExists()
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

        private bool IsFileEmpty()
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

        private void WriteDefaultFile()
        {
            if (DoesFileExists())
            {
                File.WriteAllText(FilePath, DefaultJson.ToString());
            }
        }

        private void EnsureExists()
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

        private bool SaveToFile(List<User> users, List<string> channels)
        {
            EnsureExists();
            if(users!=null && channels != null)
            {
                SaveContainer x = new()
                {
                    MyChannels = channels,
                    MyUsers = users
                };
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(x));
                return true;
            }
            return false;
        }
        private SaveContainer ReadFromFile()
        {
            EnsureExists();
            SaveContainer s = JsonConvert.DeserializeObject<SaveContainer>(File.ReadAllText(FilePath));
            return s;
        }
        /// <summary>
        /// Loads the file information to the app
        /// </summary>
        /// <param name="users">The list with all current accounts</param>
        /// <param name="channels">The list with all subscribed channels</param>
        public void Load(out List<User> users, out List<string> channels)
        {
            SaveContainer s = ReadFromFile();
            users = s.MyUsers;
            channels = s.MyChannels;
        }
        /// <summary>
        /// Saves all the app information inside the JSON file
        /// </summary>
        /// <param name="users">The list with all current accounts</param>
        /// <param name="channels">The list with all subscribed channels</param>
        public void Save(List<User> users, List<string> channels)
        {
            SaveToFile(users, channels);
        }
    }
}
