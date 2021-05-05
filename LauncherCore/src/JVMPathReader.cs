using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public static readonly Regex JavaVersionStringPattern = new Regex(@"[1-9][0-9]*((\.0)*\.[1-9][0-9]*)*");
        /// <summary>
        /// Define as the laast
        /// </summary>
        public static readonly Version LegacyJVMVersion = new Version("1.8");

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

            GetJVMsFromEnvironmentVar(appType);
            GetJVMsFromRegistry(appType);

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
        private void GetJVMsFromEnvironmentVar(Launcher.ApplicationType appType)
        {
            var jvmExecutable = appType == Launcher.ApplicationType.Console ? "java.exe" : "javaw.exe";
            var envVar = Environment.GetEnvironmentVariable("path").Split(";");

            foreach (var itr in envVar)
            {
                var path = Path.Join(itr, jvmExecutable);
                if (!File.Exists(path)) continue;
                try
                {
                    var version = new Version(JavaVersionStringPattern.Match(path).Value);
                    if (!installedJvms.ContainsKey(version))
                        installedJvms.Add(version, new SortedSet<string>());
                    installedJvms[version].Add(path);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Extract installed JVMs from Windows registry.
        /// </summary>
        /// <param name="appType">Define the type of application.</param>
        private void GetJVMsFromRegistry(Launcher.ApplicationType appType)
        {
            string jvmExecutable = appType == Launcher.ApplicationType.Console ? "java.exe" : "javaw.exe";
            using var regJavaSoft = Registry.LocalMachine.OpenSubKey("Software\\JavaSoft");
            using var regJDK = regJavaSoft.OpenSubKey("JDK");
            using var regJRE = regJavaSoft.OpenSubKey("JRE");
            using var regJDK2 = regJavaSoft.OpenSubKey("Java Development Kit");
            using var regJRE2 = regJavaSoft.OpenSubKey("Java Runtime Environment");

            ParseJVMRegKey(regJDK, jvmExecutable);
            ParseJVMRegKey(regJRE, jvmExecutable);
            ParseJVMRegKey(regJDK2, jvmExecutable);
            ParseJVMRegKey(regJRE2, jvmExecutable);
        }

        /// <summary>
        /// Parse Oracle registry key and add them to discovered list.
        /// </summary>
        /// <param name="jvmKey">Registry key to parse.</param>
        private void ParseJVMRegKey(RegistryKey jvmKey, string exeName)
        {
            if (jvmKey == null) return;

            foreach (var itr in jvmKey.GetSubKeyNames())
            {
                try
                {
                    var version = new Version(JavaVersionStringPattern.Match(itr).Value);
                    if ((from ch in itr where ch == '.' select ch).Count() <= 1)
                        continue;

                    if (version <= LegacyJVMVersion)
                    {
                        var strVersion =  Regex.Replace(itr.Replace('_', '.'), @"^1\.", "");
                        version = new Version(strVersion);
                    }

                    if (!installedJvms.ContainsKey(version))
                        installedJvms.Add(version, new SortedSet<string>());
                    var javaHome = jvmKey.OpenSubKey(itr).GetValue("JAVAHOME").ToString();
                    var jvmPath = Path.Combine(javaHome, "bin", exeName);
                    installedJvms[version].Add(jvmPath);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
