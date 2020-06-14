using System;
using System.Diagnostics;
using LauncherCore;
using Microsoft.Win32;

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

            Console.WriteLine("\nRegistry:");
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft");

            string[] keys = key.GetSubKeyNames();
            bool[] keysReg =
            {
                Array.Exists(keys, element => element == "JDK"),
                Array.Exists(keys, element => element == "JRE"),
                Array.Exists(keys, element => element == "Java Runtime Environment"),
                Array.Exists(keys, element => element == "Java Development Kit")
            };

            foreach (bool exist in keysReg)
            {
                Console.Write(string.Format("{0} ", exist));
            }

            Console.WriteLine("");

            foreach (string subkey in key.GetSubKeyNames())
            {
                Console.WriteLine(subkey);
            }

            string value = (string)key.OpenSubKey("JDK\\11.0.4.11").GetValue("JavaHome");
            Console.WriteLine(value);

            Console.WriteLine("\nJVM Version test");
#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
