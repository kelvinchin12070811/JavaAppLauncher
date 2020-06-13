using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LauncherCore.src
{
    /// <summary>
    /// Class that use to find avaliable jvms on client.
    /// </summary>
    class JVMVersionHandler
    {
        /// <summary>
        /// Class that represent the version of JVM.
        /// See official documentation of Version-String for detailed reference.
        /// https://docs.oracle.com/javase/10/install/version-string-format.htm#JSJIG-GUID-DCA60310-6565-4BB6-8D24-6FF07C1C4B4E
        /// </summary>
        public class JVMVesion
        {
            public int Feature { get; private set; } = 0;
            public int Interim { get; private set; } = 0;
            public int Update { get; private set; } = 0;
            public int Patch { get; private set; } = 0;

            /// <summary>
            /// Create JVMVersion from seperated data.
            /// </summary>
            /// <param name="feature">Feature section of JVM Version.</param>
            /// <param name="interim">Interim section of JVM Version.</param>
            /// <param name="update">Update section of JVM Version.</param>
            /// <param name="patch">Patch section of JVM Version.</param>
            public JVMVesion(int feature = 0, int interim = 0, int update = 0, int patch = 0)
            {
                Feature = feature;
                Interim = interim;
                Update = update;
                Patch = patch;
            }

            /// <summary>
            /// Create JVMVersion from Version-String.
            /// </summary>
            /// <param name="version">Version-String.</param>
            public JVMVesion(string version)
            {
                VersionParser(version);
            }

            public override string ToString()
            {
                if (Patch != 0)
                    return string.Format("{0}.{1}.{2}.{3}", Feature, Interim, Update, Patch);

                return string.Format("{0}.{1}.{2}", Feature, Interim, Update);
            }

            /// <summary>
            /// Parse JVM version to latest syntax with tech spec.
            /// </summary>
            /// <param name="version">Version number to parse</param>
            private void VersionParser(string version)
            {
                var java8VersionFormat = new Regex("^1\\.(\\d+)\\.(\\d+)_(\\d+)");
                var matchResult = java8VersionFormat.Match(version);

                if (!matchResult.Success)
                {
                    var java9VersionFormat = new Regex("^(\\d+)\\.(\\d+)\\.(\\d+)(\\.(\\d+))?");
                    matchResult = java9VersionFormat.Match(version);
                }

                if (matchResult.Success)
                {
                    var matchGroups = matchResult.Groups;
                    Feature = int.Parse(matchGroups[1].Value);
                    Interim = int.Parse(matchGroups[2].Value);
                    Update = int.Parse(matchGroups[3].Value);
                    if (matchGroups[5].Value != "")
                        Patch = int.Parse(matchGroups[5].Value);
                }
            }
        }

        /// <summary>
        /// Information about the JVM.
        /// </summary>
        public class JVMInfo
        {
            /// <summary>
            /// Determine if the JVM exist on client.
            /// </summary>
            public bool Exist = false;
            /// <summary>
            /// Determine the path of the JVM's JavaHome path.
            /// </summary>
            public string Path = null;
            /// <summary>
            /// Determine the version of the JVM, formated in x.y.z even with java 8 or bellow.
            /// </summary>
            public JVMVesion Version = null;
        }

        /// <summary>
        /// Minimum JVM version required to execute the program.
        /// </summary>
        private string minJVMVersion = null;
        /// <summary>
        /// Maximum JVM version required to execute the program.
        /// </summary>
        private string maxJVMVersion = null;

        /// <summary>
        /// Create new instance of JVMVersionHandler.
        /// </summary>
        /// <param name="minJVMVersion">Minimum JVM Version required, null to ignore min version checking</param>
        /// <param name="maxJVMVersion">Maximum JVM Version required, null to ignore max version checking</param>
        public JVMVersionHandler(string minJVMVersion, string maxJVMVersion)
        {
            this.minJVMVersion = minJVMVersion;
            this.maxJVMVersion = maxJVMVersion;
        }

        /// <summary>
        /// Find the default JVM located on the client.
        /// </summary>
        /// <returns>JVMInfo about the default JVM in client's path.</returns>
        public JVMInfo getDefaultJVM()
        {
            JVMInfo defaultJVM = new JVMInfo();
            Process defJVM = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "java",
                    Arguments = "-version",
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            defJVM.Start();
            string output = defJVM.StandardError.ReadToEnd();
            defJVM.WaitForExit();

            Regex verMatcher = new Regex("version \"(.+)\"");
            Match result = verMatcher.Match(output);

            if (result.Success)
            {
                defaultJVM.Exist = true;
                defaultJVM.Path = "java";
                //defaultJVM.Version = new JVMVesion(result.Groups[1].Value);
                defaultJVM.Version = new JVMVesion("1.8.0_165");
            }

            return defaultJVM;
        }
    }
}
