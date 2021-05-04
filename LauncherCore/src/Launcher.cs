using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using System.Collections.Generic;

namespace LauncherCore
{
    /// <summary>
    /// Singleton object to manage launcher.
    /// </summary>
    public class Launcher
    {
        /// <summary>
        /// Define application type
        /// </summary>
        public enum ApplicationType
        {
            /// <summary>
            /// Define the application as console application.
            /// </summary>
            Console,
            /// <summary>
            /// Define the application as windowed application.
            /// </summary>
            Windowed
        };

        /// <summary>
        /// Singleton instance of the launcher object.
        /// </summary>
        public static Launcher Instance => Instance_;
        private static readonly Launcher Instance_ = new Launcher();

        /// <summary>
        /// Represented as the type of application.
        /// </summary>
        public ApplicationType AppType { get; private set; }
        /// <summary>
        /// Represented as the arguments that passed into the application.
        /// </summary>
        public string[] Args { get; private set; }

        private object configDocument = null;

        private const string DefaultDocument = @"jvm:
  min version:
  max version:
  args:
  use bundled jvm: false
  jvm download path:
app:
  args:
  version:";

        /// <summary>
        /// Initialize launcher.
        /// </summary>
        /// <param name="args">Arguments passed into the application</param>
        /// <param name="appType">Define the application type</param>
        public void InitLauncher(string[] args, ApplicationType appType)
        {
            Args = args;
            AppType = appType;

            var assemPath = Assembly.GetEntryAssembly().Location;
            var configPath = string.Format("{0}/.appconfig.yaml", Path.GetDirectoryName(assemPath));

            try
            {
                using var configReader = new StreamReader(configPath);
                configDocument = new Deserializer().Deserialize(configReader);
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Launcher configuration not found.");
            }
        }

        /// <summary>
        /// Select JVM and start the application.
        /// </summary>
        public void LaunchApplication()
        {
            var pathReader = new JVMPathReader();
            var path = pathReader.GetOptimalJVM(GetMinimumJVMVersion(), GetMaximumJVMVersion());
            Console.WriteLine("Found JVM at: {0}", path);
        }

        /// <summary>
        /// Get minimum JVM version required.
        /// </summary>
        /// <returns>JVM version, null if not specified.</returns>
        public Version GetMinimumJVMVersion()
        {
            var version = (from section in (IDictionary<object, object>)configDocument
                           where (string)section.Key == "jvm"
                           from value in (IDictionary<object, object>)section.Value
                           where (string)value.Key == "min version"
                           select value.Value).FirstOrDefault()?.ToString();
            return version == null ? null : new Version(version);
        }

        /// <summary>
        /// Get maximum JVM version required.
        /// </summary>
        /// <returns>JVM version, null if not specified.</returns>
        public Version GetMaximumJVMVersion()
        {
            var version = (from section in (IDictionary<object, object>)configDocument
                           where (string)section.Key == "jvm"
                           from value in (IDictionary<object, object>)section.Value
                           where (string)value.Key == "max version"
                           select value.Value).FirstOrDefault()?.ToString();
            return version == null ? null : new Version(version);
        }

        /// <summary>
        /// Get Arguments that will be passed into JVM on launch.
        /// </summary>
        /// <returns>Arguments that will be passed into JVM or null if not specified.</returns>
        public string GetJVMArgs() => (from section in (IDictionary<object, object>)configDocument
                                       where (string)section.Key == "jvm"
                                       from value in (IDictionary<object, object>)section.Value
                                       where (string)value.Key == "args"
                                       select value.Value).FirstOrDefault().ToString();

        /// <summary>
        /// Determine if should use bundled JVM.
        /// </summary>
        /// <returns>True if use bundled JVM, false otherwise.</returns>
        public bool IsUsingBundledJVM()
        {
            var result = (from section in (IDictionary<object, object>)configDocument
                            where (string)section.Key == "jvm"
                            from value in (IDictionary<object, object>)section.Value
                            where (string)value.Key == "use bundled jvm"
                            select value.Value).FirstOrDefault();

            if (result == null) return false;
            if (result.ToString() == "true") return true;
            return false;
        }

        /// <summary>
        /// Get application version.
        /// </summary>
        /// <returns>Application version in string or null if not specified.</returns>
        public string GetAppVersion() => (from section in (IDictionary<object, object>)configDocument
                                          where (string)section.Key == "app"
                                          from value in (IDictionary<object, object>)section.Value
                                          where (string)value.Key == "version"
                                          select value.Value).FirstOrDefault()?.ToString();

        /// <summary>
        /// Get arguments that will be passed into application
        /// </summary>
        /// <returns>String of arguments will be passed into application, null if not specified.</returns>
        public string GetAppArgs() => (from section in (IDictionary<object, object>)configDocument
                                       where (string)section.Key == "app"
                                       from value in (IDictionary<object, object>)section.Value
                                       where (string)value.Key == "args"
                                       select value.Value).FirstOrDefault()?.ToString();

        /// <summary>
        /// Get specified URL to download JVM for the application.
        /// </summary>
        /// <returns>URL to download JVM or null if not specified.</returns>
        public string GetJVMDlPath() => (from section in (IDictionary<object, object>)configDocument
                                         where (string)section.Key == "jvm"
                                         from value in (IDictionary<object, object>)section.Value
                                         where (string)value.Key == "jvm download path"
                                         select value.Value).FirstOrDefault()?.ToString();

        /// <summary>
        /// Get alternate JVM path which specified in the config file, path of JVM must point to bin folder fo JVM
        /// Distribution.
        /// </summary>
        /// <returns>Path to bin folder of JVM distribution or null if not specified.</returns>
        public string GetAlternateJVMPath() => (from section in (IDictionary<object, object>)configDocument
                                                where (string)section.Key == "jvm"
                                                from value in (IDictionary<object, object>)section.Value
                                                where (string)value.Key == "alternate jvm path"
                                                select value.Value).FirstOrDefault()?.ToString();
    }
}
