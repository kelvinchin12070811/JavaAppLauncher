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
        }
    }
}
