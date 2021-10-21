using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitchPointsFarmer.Utils
{
    public static class AutoUpdater
    {
        public static Version Version { get; set; }
        public static bool CheckForUpdates()
        {
            WebClient webClient = new();
            string json = webClient.DownloadString("https://raw.githubusercontent.com/vin350/TwitchPointsFarmer/master/TwitchPointsFarmer/UpdateInfo.json");
            JObject jobj = JObject.Parse(json);
            int major = jobj["latestVersion"]["major"].ToObject<int>();
            int minor = jobj["latestVersion"]["minor"].ToObject<int>();
            Version v = new(major, minor);
            if (Version < v)
            {
                string downloadurl = jobj["setupLocation"].ToObject<string>();
                webClient.DownloadFile(downloadurl, @"C:\Temp\TwitchPointsFarmerAutoUpdateSetup.exe");
                Process.Start(@"C:\Temp\TwitchPointsFarmerAutoUpdateSetup.exe");
                return true;
            }
            return false;
        }
    }
}
