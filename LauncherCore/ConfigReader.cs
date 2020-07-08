using YamlDotNet.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace LauncherCore
{
    class ConfigReader
    {
        public LauncherConfig Config { get; private set; } = null;
        public ConfigReader()
        {
            var path = "./io.gitlab.kelvinchin12070811/javaapplauncher/launcher.config.yaml";
            var cfgFile = new StreamReader(File.OpenRead(path));

            var decerealEngine = new DeserializerBuilder().Build();
            Config = decerealEngine.Deserialize<LauncherConfig>(cfgFile);

            Console.WriteLine(Config.JVM.StartupCMD);
            Console.WriteLine(Config.App.DefaultArgs);
        }
    }
}
