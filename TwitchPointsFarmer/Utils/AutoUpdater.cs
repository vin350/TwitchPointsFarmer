using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TwitchPointsFarmer.Utils
{
    public class AutoUpdater
    {
        public AutoUpdater(Version version,MainWindow main)
        {
            Main = main;
            Version = version;
        }
        public MainWindow Main { get; set; }
        public Version Version { get; set; }
        public bool CheckForUpdates()
        {
            WebClient webClient = new();
            Main.Log("Checking for new versions...");
            string json = webClient.DownloadString("https://raw.githubusercontent.com/vin350/TwitchPointsFarmer/master/TwitchPointsFarmer/UpdateInfo.json");
            JObject jobj = JObject.Parse(json);
            int major = jobj["latestVersion"]["major"].ToObject<int>();
            int minor = jobj["latestVersion"]["minor"].ToObject<int>();
            Version v = new(major, minor);
            
            if (Version < v)
            {
                Main.Log($"Outdated version, available is {v}");
                string downloadurl = jobj["setupLocation"].ToObject<string>();
                Main.Log("Starting download...");
                webClient.DownloadFile(downloadurl, @"C:\Temp\TwitchPointsFarmerAutoUpdateSetup.exe");
                Main.Log("Download complete! Starting new Setup");
                Process.Start(@"C:\Temp\TwitchPointsFarmerAutoUpdateSetup.exe");
                return true;
            }
            Main.Log("The app is up to date!");
            return false;
        }
    }
}
