/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using System;

namespace LauncherCore
{
    public class Launcher
    {
        public enum LaunchType {
            console,
            window
        };

        public const string VERSION = "1.0";

        public void Launch(LaunchType launchType, string[] args = null)
        {
            JVMVersionHandler verHandler = new JVMVersionHandler();
            var jvmInfo = verHandler.getDefaultJVM();
            if (jvmInfo.Exist)
            {
                Console.WriteLine(jvmInfo.Version);
                Console.WriteLine(jvmInfo.Path);
            }

            ConfigReader reader = new ConfigReader();
            JVMInfo selectedJVM = verHandler.GetPrefferedJVM(reader.Config.Launcher);
            if (selectedJVM == null)
                Console.WriteLine("no prefered jvm found");
            else
                Console.WriteLine($"selected jvm: {selectedJVM.Path}");
        }
    }
}
