﻿using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using System.Collections.Generic;

namespace LauncherCore
{
    /// <summary>
    /// Singleton object to manage launcher.
    /// </summary>
    public class Launcher
    {
        /// <summary>
        /// Define application type
        /// </summary>
        public enum ApplicationType
        {
            /// <summary>
            /// Define the application as console application.
            /// </summary>
            Console,
            /// <summary>
            /// Define the application as windowed application.
            /// </summary>
            Windowed
        };

        /// <summary>
        /// Singleton instance of the launcher object.
        /// </summary>
        public static Launcher Instance => Instance_;
        private static readonly Launcher Instance_ = new Launcher();

        /// <summary>
        /// Represented as the type of application.
        /// </summary>
        public ApplicationType AppType { get; private set; }
        /// <summary>
        /// Represented as the arguments that passed into the application.
        /// </summary>
        public string[] Args { get; private set; }

        private object configDocument = null;

        private const string DefaultDocument = @"jvm:
  min version:
  max version:
  args:
  use bundled jvm: false
app:
  args:
  version:";

        /// <summary>
        /// Initialize launcher.
        /// </summary>
        /// <param name="args">Arguments passed into the application</param>
        /// <param name="appType">Define the application type</param>
        public void InitLauncher(string[] args, ApplicationType appType)
        {
            Args = args;
            AppType = appType;

            var assemPath = Assembly.GetEntryAssembly().Location;
            var configPath = string.Format("{0}/.appconfig.yaml", Path.GetDirectoryName(assemPath));

            try
            {
                configDocument = new Deserializer().Deserialize(new StreamReader(configPath));
            }
            catch (IOException)
            {
                throw new FileNotFoundException("Launcher configuration not found.");
            }
        }

        /// <summary>
        /// Get minimum JVM version required.
        /// </summary>
        /// <returns>String of JVM version, null if not specified.</returns>
        public string GetMinimumJVMVersion() => (from section in (IDictionary<object, object>)configDocument
                                                 where (string)section.Key == "jvm"
                                                 from value in (IDictionary<object, object>)section.Value
                                                 where (string)value.Key == "min version"
                                                 select value.Value).FirstOrDefault().ToString();

        /// <summary>
        /// Get maximum JVM version required.
        /// </summary>
        /// <returns>String of JVM version, null if not specified.</returns>
        public string GetMaximumJVMVersion() => (from section in (IDictionary<object, object>)configDocument
                                                 where (string)section.Key == "jvm"
                                                 from value in (IDictionary<object, object>)section.Value
                                                 where (string)value.Key == "max version"
                                                 select value.Value).FirstOrDefault()?.ToString();

        /// <summary>
        /// Get Arguments that will be passed into JVM on launch.
        /// </summary>
        /// <returns>Arguments that will be passed into JVM or null if not specified.</returns>
        public string GetJVMArgs() => (from section in (IDictionary<object, object>)configDocument
                                       where (string)section.Key == "jvm"
                                       from value in (IDictionary<object, object>)section.Value
                                       where (string)value.Key == "args"
                                       select value.Value).FirstOrDefault().ToString();

        /// <summary>
        /// Determine if should use bundled JVM.
        /// </summary>
        /// <returns>True if use bundled JVM, false otherwise.</returns>
        public bool IsUsingBundledJVM()
        {
            var result = (from section in (IDictionary<object, object>)configDocument
                            where (string)section.Key == "jvm"
                            from value in (IDictionary<object, object>)section.Value
                            where (string)value.Key == "use bundled jvm"
                            select value.Value).FirstOrDefault();

            if (result == null) return false;
            if (result.ToString() == "true") return true;
            return false;
        }

        /// <summary>
        /// Get application version.
        /// </summary>
        /// <returns>Application version in string or null if not specified.</returns>
        public string GetAppVersion() => (from section in (IDictionary<object, object>)configDocument
                                          where (string)section.Key == "app"
                                          from value in (IDictionary<object, object>)section.Value
                                          where (string)value.Key == "version"
                                          select value.Value).FirstOrDefault()?.ToString();

        /// <summary>
        /// Get arguments that will be passed into application
        /// </summary>
        /// <returns>String of arguments will be passed into application, null if not specified.</returns>
        public string GetAppArgs() => (from section in (IDictionary<object, object>)configDocument
                                       where (string)section.Key == "app"
                                       from value in (IDictionary<object, object>)section.Value
                                       where (string)value.Key == "args"
                                       select value.Value).FirstOrDefault()?.ToString();
    }
}