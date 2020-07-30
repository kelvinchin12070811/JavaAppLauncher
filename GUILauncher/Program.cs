/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using LauncherCore;

namespace GUILauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var launcher = new Launcher();
            launcher.Launch(Launcher.LaunchType.window, args);
        }
    }
}
