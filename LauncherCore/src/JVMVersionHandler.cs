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
            public string Version = null;
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
                defaultJVM.Version = VersionParser(result.Groups[1].Value);
            }

            return defaultJVM;
        }

        /// <summary>
        /// Parse JVM version to latest syntax with tech spec.
        /// </summary>
        /// <param name="version">Version number to parse</param>
        /// <returns>Version number in format of "major.minor.update"</returns>
        private string VersionParser(string version)
        {
            string formatedVersion = version;
            var java8VersionFormat = new Regex("^1\\.(\\d+)\\.(\\d+)_(\\d+)");
            var matchResult = java8VersionFormat.Match(version);
            var matchGroups = matchResult.Groups;

            if (matchResult.Success)
                formatedVersion = string.Format("{0}.{1}.{2}",
                    matchGroups[1], matchGroups[2], matchGroups[3]);

            return formatedVersion;
        }
    }
}
