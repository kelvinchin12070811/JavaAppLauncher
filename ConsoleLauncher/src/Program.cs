using System;
using System.Collections.Generic;
using LauncherCore;

namespace ConsoleLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Launcher.Instance.InitLauncher(args, Launcher.ApplicationType.Console);

#if DEBUG
                Console.WriteLine("min jvm ver: {0}", Launcher.Instance.GetMinimumJVMVersion());
                Console.WriteLine("max jvm ver: {0}", Launcher.Instance.GetMaximumJVMVersion());
                Console.WriteLine("jvm args: {0}", Launcher.Instance.GetJVMArgs());
                Console.WriteLine("use bundled jvm: {0}", Launcher.Instance.IsUsingBundledJVM());
                Console.WriteLine("jvm download path: {0}", Launcher.Instance.GetJVMDlPath());

                Console.WriteLine("app version: {0}", Launcher.Instance.GetAppVersion());
                Console.WriteLine("app args: {0}", Launcher.Instance.GetAppArgs());
#endif
                Launcher.Instance.LaunchApplication();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }

#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#endif
        }
    }
}
