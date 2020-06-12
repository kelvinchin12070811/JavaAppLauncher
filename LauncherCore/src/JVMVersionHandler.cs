﻿using System;
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
        /// Find the default JVM's version located on the client.
        /// </summary>
        /// <returns>Version of the default JVM or null if no default JVM found.</returns>
        public string getDefaultJVMVersion()
        {
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
                return result.Groups[1].Value;
            else
                return null;
        }
    }
}