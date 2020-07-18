/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    class JVMVersionHandler
    {
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

        public JVMInfo GetPrefferedJVM(CoreConfig coreConfig)
        {
            if (coreConfig.BundledJVM)
            {
                string pwd = Assembly.GetExecutingAssembly().Location;
                pwd = Path.GetDirectoryName(pwd);
                var bundledJVM = new JVMInfo
                {
                    Exist = true,
                    Path = $"{pwd}\\runtime",
                    Version = JVMVersion.EMPTY
                };
                return bundledJVM;
            }

            if (coreConfig.SearchPath.Registry)
            {
                List<JVMInfo> registeredJVMs = GetAllRegisteredJVM();

                if (coreConfig.Version.Min == null || coreConfig.Version.Max == null)
                    return registeredJVMs.LastOrDefault();

                var selectedJvm = from jvm in registeredJVMs
                                  where jvm.Version >= coreConfig.Version.Min &&
                                  jvm.Version <= coreConfig.Version.Max
                                  select jvm;

                foreach (var jvm in selectedJvm)
                    System.Console.WriteLine($"v{jvm.Version}: {jvm.Path}");

                return selectedJvm.LastOrDefault();
            }

            if (coreConfig.SearchPath.CmdDefault)
            {
                JVMInfo defaultJvm = getDefaultJVM();
                if (coreConfig.Version.Min == null || defaultJvm.Version >= coreConfig.Version.Min)
                {
                    if (coreConfig.Version.Max == null || coreConfig.Version.Max <= defaultJvm.Version)
                        return defaultJvm;
                }
            }

            return null;
        }
    }
}
