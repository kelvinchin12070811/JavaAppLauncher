using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    /// <summary>
    /// Class that use to find avaliable jvms on client.
    /// </summary>
    class JVMVersionHandler
    {
        /// <summary>
        /// Minimum JVM version required to execute the program.
        /// </summary>
        private string minJVMVersion = null;
        /// <summary>
        /// Maximum JVM version required to execute the program.
        /// </summary>
        private string maxJVMVersion = null;

        /// <summary>
        /// Create new instance of JVMVersionHandler.
        /// </summary>
        /// <param name="minJVMVersion">Minimum JVM Version required, null to ignore min version checking</param>
        /// <param name="maxJVMVersion">Maximum JVM Version required, null to ignore max version checking</param>
        public JVMVersionHandler(string minJVMVersion, string maxJVMVersion)
        {
            this.minJVMVersion = minJVMVersion;
            this.maxJVMVersion = maxJVMVersion;
        }

        /// <summary>
        /// Find the default JVM located on the client.
        /// </summary>
        /// <returns>JVMInfo about the default JVM in client's path.</returns>
        public JVMInfo getDefaultJVM()
        {
            JVMInfo defaultJVM = new JVMInfo();
            Process defJVM = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "java",
                    Arguments = "-version",
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            defJVM.Start();
            string output = defJVM.StandardError.ReadToEnd();
            defJVM.WaitForExit();

            Regex verMatcher = new Regex("version \"(.+)\"");
            Match result = verMatcher.Match(output);

            if (result.Success)
            {
                defaultJVM.Exist = true;
                defaultJVM.Path = "";
                defaultJVM.Version = new JVMVersion(result.Groups[1].Value);
            }

            return defaultJVM;
        }

        /// <summary>
        /// Get a list of JVMs which registered in registry.
        /// </summary>
        /// <returns>List of JVM avaliable.</returns>
        public List<JVMInfo> GetAllRegisteredJVM()
        {
            var jvms = new List<JVMInfo>();

            var hkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft");
            string[] keys = hkey.GetSubKeyNames();
            var jvmLists = new Regex("(JDK)|(JRE)|(Java Runtime Environment)|(Java Development Kit)");

            foreach (string key in keys)
            {
                if (!jvmLists.Match(key).Success) continue;
                var registeredJVMs = hkey.OpenSubKey(key);
                
                foreach (string jvm in registeredJVMs.GetSubKeyNames())
                {
                    var info = new JVMInfo();
                    info.Path = (string)registeredJVMs.OpenSubKey(jvm).GetValue("JavaHome");
                    info.Version = new JVMVersion(jvm);

                    if (info.Version == new JVMVersion(0, 0, 0, 0))
                        continue;

                    jvms.Add(info);
                }
            }

            jvms.Sort(delegate(JVMInfo lhs, JVMInfo rhs)
            {
                if (lhs.Version > rhs.Version) return 1;
                else if (lhs.Version < rhs.Version) return -1;
                else return 0;
            });

            return jvms;
        }
    }
}
