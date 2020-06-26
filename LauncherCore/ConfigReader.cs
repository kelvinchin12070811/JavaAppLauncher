using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System;
using System.IO;

namespace LauncherCore
{
    /// <summary>
    /// Object that read the configuration file of the launcher.
    /// </summary>
    class ConfigReader
    {
        public LauncherConfig Config { get; private set; } = null;
        public ConfigReader()
        {
            var path = "./io.gitlab.kelvinchin12070811/javaapplauncher/launcher.config.yaml";
            var cfgFile = new StreamReader(File.OpenRead(path));

            var decerealEngine = new Deserializer();
            Config = decerealEngine.Deserialize<LauncherConfig>(cfgFile);

            Console.WriteLine(Config.Config.Version.Min);
            Console.WriteLine(Config.Config.Version.Max == null);
            Console.WriteLine(Config.Config.SearchPath.CommandLineDefault);
        }
    }
}
