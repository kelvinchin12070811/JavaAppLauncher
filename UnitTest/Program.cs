/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
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
            testResult.Add("JVM version less than(equal, false)", !(minVer < minVer));
            testResult.Add("JVM version less than & equal, less", minVer <= maxVer);
            testResult.Add("JVM version less than & equal, equal", maxVer <= maxVer);
            testResult.Add("JVM version less than & equal, false", !(maxVer <= minVer));
            testResult.Add("JVM version greather than", maxVer > minVer);
            testResult.Add("JVM version greather than(false)", !(minVer > maxVer));
            testResult.Add("JVM version greather than(equal)", !(maxVer > maxVer));
            testResult.Add("JVM version greather than & equal, larger", maxVer >= minVer);
            testResult.Add("JVM version greather than & equal, equal", maxVer >= maxVer);
            testResult.Add("JVM version greather than & equal, false", !(minVer >= maxVer));
#pragma warning restore CS1718 // Comparison made to same variable
        }

        static void Main(string[] args)
        {
            testResult = new Dictionary<string, bool>();

            VersionTest();

            int passed = 0;
            foreach (var result in testResult)
            {
                if (result.Value == false)
                    Console.Write("->");

                Console.WriteLine($"{result.Key}: {(result.Value ? "Passed" : "Failed")}");
                if (result.Value == true) passed++;
            }

            Console.WriteLine($"\n\nTotal test: {testResult.Count}");
            Console.WriteLine($"Passed test: {passed}");
            Console.WriteLine($"Failed test: {testResult.Count - passed}");

            Console.ReadKey();
        }
    }
}
