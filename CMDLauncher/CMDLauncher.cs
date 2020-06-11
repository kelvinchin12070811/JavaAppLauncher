using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMDLauncher
{
    class CMDLauncher
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");
            Console.WriteLine(string.Format("Lib version {0}", LauncherCore.Launcher.VERSION));
        }
    }
}
