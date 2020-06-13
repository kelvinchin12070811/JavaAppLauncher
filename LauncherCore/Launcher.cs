using System;

namespace LauncherCore
{
    public class Launcher
    {
        public enum LaunchType { console, window };

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
        }
    }
}
