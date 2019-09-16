//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Launcher
{
    class ConfigReader
    {
        public const string DEF_APP_NAME = "Kelvin's Java App Launcher";
        public const string LAUNCHER_CFG_FILE = "javaapplauncher.cfg";

        public string Appname { get; private set; }
        public bool BundledJvm { get; private set; }
        public string Jarfile { get; private set; }
        public string MaxJvmVersion { get; private set; }
        public string MinJvmVersion { get; private set; }
        public string JvmArgs { get; private set; }
        public string JvmDlPath { get; private set; } = "";
        public string JvmPath { get; private set; }

        public enum Runtype { console, window };

        public ConfigReader()
        {
            string exefile = "";
            string jsonAppInfo = ReadLauncherCfg(ref exefile);
            
            //var appinfo = JsonConvert.DeserializeObject(jsonAppInfo) as JObject;
            //Appname = GetContent(appinfo["app"]["name"], DEF_APP_NAME);
            //BundledJvm = GetContent(appinfo["jvm"]["bundled"], "").Equals("true") ? true : false;
            //MinJvmVersion = GetContent(appinfo["jvm"]["version"]["min"]);
            //MaxJvmVersion = GetContent(appinfo["jvm"]["version"]["max"]);
            //JvmArgs = GetContent(appinfo["jvm"]["args"], "");
            //JvmDlPath = GetContent(appinfo["jvm"]["url"], "https://www.adoptopenjdk.net");
            //JvmPath = GetContent(appinfo["jvm"]["jvm_path"]);
        }
        
        public string GetJvmPath()
        {
            if (BundledJvm) return GetApplicationDirectory() + "/jvm/bin/javaw.exe";
            else if (JvmPath != null) return JvmPath;

            return "javaw.exe";
        }

        private string ReadLauncherCfg(ref string exefile)
        {
            var appPath = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            exefile = appPath + ".exe";
            Jarfile = appPath + ".jar";
            if (!File.Exists(Jarfile))
                Jarfile = exefile;

            long szCfgFile = 0;
            DeflateStream appcfg = IndexLauncherCfg(ref szCfgFile);
            if (appcfg == null)
                throw new Exception("Error read launcher config from " + Jarfile);

            return new StreamReader(appcfg).ReadToEnd();
        }

        private DeflateStream IndexLauncherCfg(ref long szCfgFile)
        {
            long szJar = 0;
            long szCdir = 0;
            long posCdir = 0;
            long posActJarOffset = 0;
            int ctnCdir = 0;

            BinaryReader jarReader = new BinaryReader(File.Open(Jarfile, FileMode.Open));
            szJar = jarReader.BaseStream.Length;

            jarReader.BaseStream.Seek(szJar - 22, SeekOrigin.Begin);
            if (jarReader.ReadUInt32() != 0x06054b50) // Find end of center directory.
                throw new Exception("Error on finding zip's end of center directory");

            jarReader.BaseStream.Seek(3 * 2, SeekOrigin.Current);
            ctnCdir = jarReader.ReadUInt16();
            szCdir = jarReader.ReadUInt32();
            posCdir = jarReader.ReadUInt32();

            // Calculate offset of jar file in binary incase of it at behind of exe file.
            posActJarOffset = (szJar - 22 - szCdir) - posCdir;

            posCdir += posActJarOffset;
            jarReader.BaseStream.Seek(posCdir, SeekOrigin.Begin);

            UInt32 posCfgHeader = 0;
            UInt16 lengFileComment = 0;
            UInt16 lengFileName = 0;
            UInt16 lengExtraField = 0;
            UInt16 compressType = 0;
            UInt32 szCompressed = 0;
            UInt32 szUncompressed = 0;
            string filename = null;

            for (int idx = 0; idx <= ctnCdir; idx++)
            {
                if (idx >= ctnCdir)
                    throw new Exception("Unable to find " + LAUNCHER_CFG_FILE);

                if (jarReader.ReadUInt32() != 0x02014b50)
                    throw new Exception("Error on finding center directory");

                jarReader.BaseStream.Seek(3 * 2, SeekOrigin.Current);
                compressType = jarReader.ReadUInt16();

                jarReader.BaseStream.Seek(8, SeekOrigin.Current);
                szCompressed = jarReader.ReadUInt32();
                szUncompressed = jarReader.ReadUInt32();
                lengFileName = jarReader.ReadUInt16();
                lengExtraField = jarReader.ReadUInt16();
                lengFileComment = jarReader.ReadUInt16();

                jarReader.BaseStream.Seek(8, SeekOrigin.Current);
                posCfgHeader = jarReader.ReadUInt32();
                filename = new string(jarReader.ReadChars(lengFileName));

                if (filename == LAUNCHER_CFG_FILE)
                {
                    if (compressType != 8)
                        throw new Exception("The launcher config file is not compressed as deflated");

                    break;
                }

                jarReader.BaseStream.Seek(lengExtraField + lengFileComment, SeekOrigin.Current);
            }

            jarReader.BaseStream.Seek(posCfgHeader + posActJarOffset, SeekOrigin.Begin);
            if (jarReader.ReadUInt32() != 0x04034b50)
                throw new Exception("Error on finding local header of launcher config file");

            jarReader.BaseStream.Seek(12 * 2, SeekOrigin.Current);
            lengExtraField = jarReader.ReadUInt16();
            jarReader.BaseStream.Seek(lengFileName + lengExtraField, SeekOrigin.Current);

            szCfgFile = szUncompressed;
            return new DeflateStream(jarReader.BaseStream, CompressionMode.Decompress);
        }

        private string GetApplicationDirectory()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(path);
        }
    }
}
