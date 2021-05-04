using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    /// <summary>
    /// Object to find avaliable JVMs on device.
    /// </summary>
    public class JVMPathReader
    {
        /// <summary>
        /// Determine the pattern of the java version string, refer https://openjdk.java.net/jeps/223 for more info.
        /// </summary>
        private const string JavaVersionStringPattern = @"[1-9][0-9]*((\.0)*\.[1-9][0-9]*)*";

        /// <summary>
        /// Lists of installed JVMs on current device.
        /// </summary>
        private SortedList<string, SortedSet<string>> installedJvms = new SortedList<string, SortedSet<string>>();

        /// <summary>
        /// Find optimal JVM avaliable on the system to launch the application.
        /// </summary>
        /// <param name="minVer">Minimum version of JVM to find.</param>
        /// <param name="maxVer">Maximum version of JVM to find.</param>
        /// <returns>Path to JVM executable (java.exe or javaw.exe) or null if no JVM found.</returns>
        public string GetOptimalJVM(string minVer, string maxVer)
        {
            var appType = Launcher.Instance.AppType;
            var jvmExeName = appType == Launcher.ApplicationType.Console ? "java.exe" : "javaw.exe";

            if (Launcher.Instance.IsUsingBundledJVM())
            {
                var path = Assembly.GetEntryAssembly().Location;
                var dirPath = Path.GetDirectoryName(path);
                var jvmPath = Path.Join(dirPath, "runtime", "jvm", "bin", jvmExeName);
                return File.Exists(jvmPath) ? jvmPath : null;
            }

            var altJvmPath = Path.Join(Launcher.Instance.GetAlternateJVMPath(), jvmExeName);
            if (altJvmPath != null)
            {
                return File.Exists(altJvmPath) ? altJvmPath : null;
            }

            return null;
        }

        /// <summary>
        /// Get the default JVM located in the environment variables
        /// </summary>
        /// <param name="minVersion">Version of JVM to search.</param>
        /// <param name="maxVersion">Maximum version of JVM to search.</param>
        /// <param name="appType">Type of the application, determine which type of JVM to search.</param>
        private void GetEnvJVM(string minVersion, string maxVersion, Launcher.ApplicationType appType)
        {
            var jvmExecutable = appType == Launcher.ApplicationType.Console ? "java.exe" : "javaw.exe";
            var envVar = Environment.GetEnvironmentVariable("path").Split(";");
            var jvms = new SortedList<string, SortedSet<string>>();
            
            foreach (var itr in envVar)
            {
                var path = string.Format("{0}\\{1}", itr, jvmExecutable);
                if (!File.Exists(path)) continue;
                var version = new Regex(JavaVersionStringPattern).Match(path).Value;

                if (!jvms.ContainsKey(version))
                    jvms.Add(version, new SortedSet<string>());


            }
        }
    }
}
