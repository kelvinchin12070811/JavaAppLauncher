using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace Launcher
{
    class ConfigReader
    {
        public string Appname { get; private set; }
        public bool BundledJvm { get; private set; }
        public string Filename { get; private set; }
        public string MaxJvmVersion { get; private set; }
        public string MinJvmVersion { get; private set; }
        public string JvmArgs { get; private set; }
        public string JvmDlPath { get; private set; }
        public string JvmPath { get; private set; }

        public enum Runtype { console, window };

        public ConfigReader()
        {
            string jsonAppInfo = ReadAppInfo();
            if (jsonAppInfo == null)
                throw new Exception("Error getting application info.");

            var appinfo = JsonConvert.DeserializeObject(jsonAppInfo) as JObject;
            Appname = GetContent(appinfo["app"]["name"]);
            Filename = GetContent(appinfo["app"]["jar"]);
            BundledJvm = GetContent(appinfo["jvm"]["bundled"], "").Equals("true") ? true : false;
            MinJvmVersion = GetContent(appinfo["jvm"]["version"]["min"]);
            MaxJvmVersion = GetContent(appinfo["jvm"]["version"]["max"]);
            JvmArgs = GetContent(appinfo["jvm"]["args"], "");
            JvmDlPath = GetContent(appinfo["jvm"]["url"], "https://www.adoptopenjdk.net");
            JvmPath = GetContent(appinfo["jvm"]["jvm_path"]);
        }
        
        public string GetJvmPath()
        {
            if (BundledJvm) return Directory.GetCurrentDirectory() + "/jvm/bin/javaw.exe";
            else if (JvmPath != null) return JvmPath;

            return "javaw.exe";
        }

        private string GetContent(JToken obj, string def = null)
        {
            return obj == null ? def : obj.ToString();
        }

        private string ReadAppInfo()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "/applaunchercfg.json"))
                return null;

            Filename = Filename + Directory.GetCurrentDirectory();
            return File.ReadAllText(Directory.GetCurrentDirectory() + "/applaunchercfg.json");
        }
    }
}
