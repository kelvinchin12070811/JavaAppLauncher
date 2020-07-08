using YamlDotNet.Serialization;

namespace LauncherCore
{
    public sealed class VersionRange
    {
        [YamlMember(Alias = "min", ApplyNamingConventions = false)]
        public string Min { get; set; } = "";
        [YamlMember(Alias = "max", ApplyNamingConventions = false)]
        public string Max { get; set; } = "";
    }

    public sealed class SearchPath
    {
        [YamlMember(Alias = "registry", ApplyNamingConventions = false)]
        public bool Registry { get; set; } = false;
        [YamlMember(Alias = "env var", ApplyNamingConventions = false)]
        public bool EnvVar { get; set; } = false;
        [YamlMember(Alias = "cmd default", ApplyNamingConventions = false)]
        public bool CmdDefault { get; set; } = false;
    }

    public sealed class CoreConfig
    {
        [YamlMember(Alias = "version", ApplyNamingConventions = false)]
        public VersionRange Version { get; set; } = null;
        [YamlMember(Alias = "search path", ApplyNamingConventions = false)]
        public SearchPath SearchPath { get; set; } = null;
        [YamlMember(Alias = "bundled jvm", ApplyNamingConventions = false)]
        public bool BundledJVM { get; set; } = false;
    }

    public sealed class JVMConfig
    {
        public JVMConfig()
        {
        }
        [YamlMember(Alias = "vm args", ApplyNamingConventions = false)]
        public string VmArgs { get; set; } = "";
        [YamlMember(Alias = "startup cmd", ApplyNamingConventions = false)]
        public string StartupCMD { get; set; } = "";
    }

    public sealed class AppConfig
    {
        [YamlMember(Alias = "default args", ApplyNamingConventions = false)]
        public string DefaultArgs { get; set; } = "";
    }

    public sealed class LauncherConfig
    {
        [YamlMember(Alias = "launcher", ApplyNamingConventions = false)]
        public CoreConfig Launcher { set; get; } = null;
        [YamlMember(Alias = "jvm", ApplyNamingConventions = false)]
        public JVMConfig JVM { set; get; } = null;
        [YamlMember(Alias = "app", ApplyNamingConventions = false)]
        public AppConfig App { set; get; } = null;
    }
}
