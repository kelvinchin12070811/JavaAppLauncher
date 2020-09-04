/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using System;
using System.Diagnostics;
using System.Text;

namespace LauncherCore
{
    public class Launcher
    {
        public enum LaunchType
        {
            console,
            window
        };

        public const string VERSION = "1.0";

        private JVMInfo selectedJVM = null;
        private LauncherConfig launcherConfig = null;
        private LaunchType launchType;
        private string[] args = null;

        public void Launch(LaunchType launchType, string[] args = null)
        {
            this.launchType = launchType;
            this.args = args;

            var verHandler = new JVMVersionHandler();
            var reader = new ConfigReader();
            launcherConfig = reader.Config;
            selectedJVM = verHandler.GetPrefferedJVM(launcherConfig.Launcher);

            if (selectedJVM == null)
            {
                Console.WriteLine("no prefered jvm found");
                return;
            }

            LaunchApp();
        }

        private void LaunchApp()
        {
            bool isJar = launcherConfig.Launcher.LaunchFile.EndsWith(".jar");
            string jvmExecutable = launchType == LaunchType.console ? "java.exe" : "javaw.exe";
            string jvmPath = $"{selectedJVM.Path}bin\\{jvmExecutable}";
            var argStrBuilder = new StringBuilder();

            foreach (var arg in args)
                argStrBuilder.Append($"\"{arg}\" ");

            var argStr = argStrBuilder.ToString().Trim();

            Process jvm;
            if (isJar)
            {
                jvm = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = jvmPath,
                        Arguments = $"{launcherConfig.JVM.VmArgs} -jar {launcherConfig.Launcher.LaunchFile} {argStr}",
                        UseShellExecute = launchType == LaunchType.window
                    }
                };
            }
            else
            {
                jvm = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = jvmPath,
                        Arguments = $"{launcherConfig.JVM.VmArgs} {launcherConfig.Launcher.LaunchFile} {argStr}",
                        UseShellExecute = launchType == LaunchType.window
                    }
                };
            }

            jvm.Start();
            if (launchType == LaunchType.console) jvm.WaitForExit();
        }
    }
}
