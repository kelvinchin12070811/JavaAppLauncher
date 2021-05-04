using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private SortedList<Version, SortedSet<string>> installedJvms = new SortedList<Version, SortedSet<string>>();

        /// <summary>
        /// Find optimal JVM avaliable on the system to launch the application.
        /// </summary>
        /// <param name="minVer">Minimum version of JVM to find.</param>
        /// <param name="maxVer">Maximum version of JVM to find.</param>
        /// <returns>Path to JVM executable (java.exe or javaw.exe) or null if no JVM found.</returns>
        public string GetOptimalJVM(Version minVer, Version maxVer)
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

            var altJvmPath = Launcher.Instance.GetAlternateJVMPath();
            if (altJvmPath != null)
            {
                altJvmPath = Path.Join(altJvmPath, jvmExeName);
                return File.Exists(altJvmPath) ? altJvmPath : null;
            }

            GetEnvJVM(appType);

            var selectedJvm = from version in installedJvms
                              where version.Key.CompareTo(minVer) >= 0
                                  && (maxVer == null || version.Key.CompareTo(maxVer) <= 0)
                              select version.Value;
            return selectedJvm.FirstOrDefault()?.First()?.ToString();
        }

        /// <summary>
        /// Get the default JVM located in the environment variables
        /// </summary>
        /// <param name="appType">Type of the application, determine which type of JVM to search.</param>
        private void GetEnvJVM(Launcher.ApplicationType appType)
        {
            var jvmExecutable = appType == Launcher.ApplicationType.Console ? "java.exe" : "javaw.exe";
            var envVar = Environment.GetEnvironmentVariable("path").Split(";");
            
            foreach (var itr in envVar)
            {
                var path = Path.Join(itr, jvmExecutable);
                if (!File.Exists(path)) continue;
                var version = new Version(new Regex(JavaVersionStringPattern).Match(path).Value);
                if (!installedJvms.ContainsKey(version))
                    installedJvms.Add(version, new SortedSet<string>());
                installedJvms[version].Add(path);
            }
        }
    }
}
