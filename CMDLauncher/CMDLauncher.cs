/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using System;
using LauncherCore;

namespace CMDLauncher
{
    class CMDLauncher
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            Console.WriteLine(string.Format("Lib version {0}", Launcher.VERSION));
            Launcher launcher = new Launcher();
            launcher.Launch(Launcher.LaunchType.console);

            Console.WriteLine("\nJVM Version test");
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
