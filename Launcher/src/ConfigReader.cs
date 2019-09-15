//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;

namespace Launcher
{
    class ConfigReader
    {
        public const string DEF_APP_NAME = "Kelvin's Java App Launcher";
        public string Appname { get; private set; }
        public bool BundledJvm { get; private set; }
        public string Jarfile { get; private set; }
        public string MaxJvmVersion { get; private set; }
        public string MinJvmVersion { get; private set; }
        public string JvmArgs { get; private set; }
        public string JvmDlPath { get; private set; }
        public string JvmPath { get; private set; }

        public enum Runtype { console, window };

        public ConfigReader()
        {
            string asmname = "";
            string jsonAppInfo = ReadAppInfo(ref asmname);

            var appinfo = JsonConvert.DeserializeObject(jsonAppInfo) as JObject;
            Appname = GetContent(appinfo["app"]["name"], DEF_APP_NAME);
            Jarfile = asmname + ".jar";
            BundledJvm = GetContent(appinfo["jvm"]["bundled"], "").Equals("true") ? true : false;
            MinJvmVersion = GetContent(appinfo["jvm"]["version"]["min"]);
            MaxJvmVersion = GetContent(appinfo["jvm"]["version"]["max"]);
            JvmArgs = GetContent(appinfo["jvm"]["args"], "");
            JvmDlPath = GetContent(appinfo["jvm"]["url"], "https://www.adoptopenjdk.net");
            JvmPath = GetContent(appinfo["jvm"]["jvm_path"]);
        }
        
        public string GetJvmPath()
        {
            if (BundledJvm) return GetApplicationDirectory() + "/jvm/bin/javaw.exe";
            else if (JvmPath != null) return JvmPath;

            return "javaw.exe";
        }

        private string GetContent(JToken obj, string def = null)
        {
            return obj == null ? def : obj.ToString();
        }

        private string ReadAppInfo(ref string asmname)
        {
            asmname = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            var infoFile = asmname + ".japplaunch.json";
            if (!File.Exists(infoFile))
                throw new Exception(string.Format("Failed to load application info from {0}", infoFile));

            return File.ReadAllText(infoFile);
        }

        private string GetApplicationDirectory()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(path);
        }
    }
}
