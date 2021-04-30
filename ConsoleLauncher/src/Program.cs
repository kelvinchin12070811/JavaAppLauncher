using System;
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
                Console.WriteLine("min jvm ver: {0}", Launcher.Instance.GetMinimumJVMVersion());
                Console.WriteLine("max jvm ver: {0}", Launcher.Instance.GetMaximumJVMVersion());
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.Message);
            }
            Console.Read();
        }
    }
}
