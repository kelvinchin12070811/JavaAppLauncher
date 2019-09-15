//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Launcher
{
    class JvmVersionChecker
    {
        private string jvmMinVersion = null;
        private string jvmMaxVersion = null;

        public JvmVersionChecker(string jvmMinVersion, string jvmMaxVersion = null)
        {
            this.jvmMinVersion = jvmMinVersion;
            this.jvmMaxVersion = jvmMaxVersion;
        }

        public bool CheckAvaliableJvmInPath()
        {
            Process defjvm = new Process();
            defjvm.StartInfo.FileName = "java";
            defjvm.StartInfo.Arguments = "-version";
            defjvm.StartInfo.UseShellExecute = false;
            defjvm.StartInfo.RedirectStandardError = true;
            defjvm.StartInfo.CreateNoWindow = true;
            defjvm.Start();
            defjvm.WaitForExit();

            string versionInfo = null;
            using (StreamReader sr = defjvm.StandardError)
            {
                if ((versionInfo = sr.ReadLine()) != null)
                {
                    Regex versionPat = new Regex("^.+?version \"(.+?)\".+?$");
                    Match res = versionPat.Match(versionInfo);
                    versionInfo = res.Groups[1].ToString();

                    if (jvmMinVersion == null || versionInfo.CompareTo(jvmMinVersion) < 0)
                        return false;

                    if (jvmMaxVersion != null && versionInfo.CompareTo(jvmMaxVersion) > 0)
                        return false;
                }
            }

            return true;
        }
    }
}
