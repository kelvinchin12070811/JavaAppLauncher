using YamlDotNet.RepresentationModel;
using System;
using System.IO;

namespace LauncherCore
{
    class ConfigReader
    {
        public class CfgStructer
        {
            public class JVM
            {
                public class Ver
                {
                    public JVMVersion min = null;
                    public JVMVersion max = null;
                }

                public bool bundle = false;
                public Ver version = null;
                public string type = "";
                public string args = "";
                public string cmd = "";
            }

            public class App
            {
                public string id = "";
                public string app = "";
            }

            public JVM jvm = null;
            public App app = null;
        }

        public CfgStructer LauncherCfg { get; private set; } = null;

        public ConfigReader()
        {
            var path = "./io/gitlab/kelvinchin12070811/javaapplauncher/launcher.config.yaml";
            var cfgFile = new StreamReader(File.OpenRead(path));
            var cfg = new YamlStream();
            cfg.Load(cfgFile);
            var document = (YamlMappingNode)cfg.Documents[0].RootNode;
            var jvm = (YamlMappingNode)document["jvm"];
            var app = (YamlMappingNode)document["app"];

            var jvmVersionExtractor = new Func<string, JVMVersion>((string key) =>
            {
                string res = ((YamlScalarNode)jvm["version"][key]).Value;
                return res == string.Empty ?
                    null :
                    new JVMVersion(res);
            });

            LauncherCfg = new CfgStructer
            {
                jvm = new CfgStructer.JVM
                {
                    bundle = new Func<bool>(() =>
                    {
                        string res = ((YamlScalarNode)jvm["bundle"]).Value;
                        return res == string.Empty ?
                            false :
                            bool.Parse(res);
                    })(),
                    version = new CfgStructer.JVM.Ver
                    {
                        min = jvmVersionExtractor("min"),
                        max = jvmVersionExtractor("max")
                    }
                }
            };

            Console.WriteLine(LauncherCfg.jvm.version.min);
            Console.WriteLine(LauncherCfg.jvm.version.max == null);
        }
    }
}
