using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    class JVMVersionHandler
    {
        private string minJVMVersion = null;
        private string maxJVMVersion = null;

        public JVMVersionHandler(string minJVMVersion, string maxJVMVersion)
        {
            this.minJVMVersion = minJVMVersion;
            this.maxJVMVersion = maxJVMVersion;
        }

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
                    var info = new JVMInfo()
                    {
                        Exist = true,
                        Path = (string)registeredJVMs.OpenSubKey(jvm).GetValue("JavaHome"),
                        Version = new JVMVersion(jvm)
                    };

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
