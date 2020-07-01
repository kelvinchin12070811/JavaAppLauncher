using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System;
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

            var decerealEngine = new Deserializer();
            Config = decerealEngine.Deserialize<LauncherConfig>(cfgFile);

            Console.WriteLine(Config.Launcher.Version.Min);
            Console.WriteLine(Config.Launcher.Version.Max == null);
            Console.WriteLine(Config.Launcher.SearchPath.CommandLineDefault);
        }
    }
}
