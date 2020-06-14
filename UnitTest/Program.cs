using LauncherCore;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    class Program
    {
        static Dictionary<string, bool> testResult = null;
        static void VersionTest()
        {
            var minVer = new JVMVersion("1.8.0_46");
            var maxVer = new JVMVersion("11.0.4");

#pragma warning disable CS1718 // Comparison made to same variable
            testResult.Add("JVM version equal", minVer == minVer);
            testResult.Add("JVM version equal(false)", !(minVer == maxVer));
            testResult.Add("JVM version not equal", minVer != maxVer);
            testResult.Add("JVM version less than", minVer < maxVer);
            testResult.Add("JVM version less than(false)", !(maxVer < minVer));
            testResult.Add("JVM version less than & equal, less", minVer <= maxVer);
            testResult.Add("JVM version less than & equal, equal", maxVer <= maxVer);
            testResult.Add("JVM version less than & equal, equal(false)", !(maxVer <= minVer));
#pragma warning restore CS1718 // Comparison made to same variable
        }

        static void Main(string[] args)
        {
            testResult = new Dictionary<string, bool>();

            VersionTest();

            int passed = 0;
            foreach (var result in testResult)
            {
                Console.WriteLine("{0}: {1}", result.Key, result.Value);
                if (result.Value == true) passed++;
            }

            Console.WriteLine("\n\nTotal test: {0}", testResult.Count);
            Console.WriteLine("Passed test: {0}", passed);
            Console.WriteLine("Failed test: {0}", testResult.Count - passed);

            Console.ReadKey();
        }
    }
}
