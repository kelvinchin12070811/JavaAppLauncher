using YamlDotNet.Serialization;

namespace LauncherCore
{
    /// <summary>
    /// Configuration of launcher.
    /// </summary>
    public class LauncherConfig
    {
        /// <summary>
        /// Core configuraiton of launcher.
        /// </summary>
        public class CoreConfig
        {
            /// <summary>
            /// Range of java version that required to run the app.
            /// </summary>
            public class VersionRange
            {
                /// <summary>
                /// Minimum JVM Version required. Null for any version.
                /// </summary>
                [YamlMember(Alias = "min")]
                public JVMVersion Min { get; set; } = null;
                /// <summary>
                /// Maximum JVM Version required. Null for any version.
                /// </summary>
                [YamlMember(Alias = "max")]
                public JVMVersion Max { get; set; } = null;
            }

            /// <summary>
            /// Define paths where the launcher to search for JVMs.
            /// Search order of the launcer will be as bellow:
            /// 1. Registry
            /// 2. Environment variable
            /// 3. Default JVM by running 'java -version' on current location of launcher.
            /// </summary>
            public class JVMSearchPath
            {
                /// <summary>
                /// Allow launcher to search registry if true.
                /// </summary>
                [YamlMember(Alias = "registry")]
                public bool Registry { get; set; } = false;
                /// <summary>
                /// Allow launcher to search environment variable if true.
                /// </summary>
                [YamlMember(Alias = "env var")]
                /// <summary>
                /// Allow launcher to verify default JVM (running 'java -version' in current path) if true.
                /// </summary>
                public bool EnvironmentVariables { get; set; } = false;
                [YamlMember(Alias = "cmd default")]
                public bool CommandLineDefault { get; set; } = false;
            }

            /// <summary>
            /// Boundary of JVM version required to run the app.
            /// </summary>
            [YamlMember(Alias = "version")]
            public VersionRange Version { get; set; } = null;
            /// <summary>
            /// Paths that launcher will search.
            /// </summary>
            [YamlMember(Alias = "search path")]
            public JVMSearchPath SearchPath { get; set; } = null;
            /// <summary>
            /// Define if JVM is bundled with the app, launcher will not search for jvm and use bundled JVM
            /// if set to true.
            /// </summary>
            [YamlMember(Alias = "bundled jvm")]
            public bool BundledJVM { get; set; } = false;
        }

        /// <summary>
        /// Configuration of the launcher.
        /// </summary>
        [YamlMember(Alias = "launcher")]
        public CoreConfig Config { get; set; } = null;
    }
}
