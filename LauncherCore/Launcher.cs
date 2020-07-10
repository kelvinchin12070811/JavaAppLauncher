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

        public void Launch(LaunchType launchType)
        {
            JVMVersionHandler verHandler = new JVMVersionHandler(null, null);
            var jvmInfo = verHandler.getDefaultJVM();
            if (jvmInfo.Exist)
            {
                Console.WriteLine(jvmInfo.Version);
                Console.WriteLine(jvmInfo.Path);
            }

            var jvms = verHandler.GetAllRegisteredJVM();
            foreach (JVMInfo info in jvms)
            {
                Console.WriteLine($"{info.Path}: {info.Version}");
            }

            ConfigReader reader = new ConfigReader();
        }
    }
}
