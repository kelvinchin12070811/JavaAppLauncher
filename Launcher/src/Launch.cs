﻿//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Launcher
{
    class Launch
    {
        static void StartApp(ConfigReader reader, string[] args)
        {
            Process jvm = new Process();
            jvm.StartInfo.FileName = reader.GetJvmPath();
            jvm.StartInfo.UseShellExecute = false;
            jvm.StartInfo.Arguments = string.Format("{0} -jar \"{1}\" {2}",
                reader.JvmArgs, reader.Jarfile, CatArgs(args));
            jvm.Start();
        }

        static string CatArgs(string[] args)
        {
            string line = "";

            foreach (var itr in args)
            {
                line += string.Format("\"{0}\" ", itr);
            }
            line = line.Substring(0, Math.Max(0, line.Length - 1));
            return line;
        }

        static void Main(string[] args)
        {
            ConfigReader cfg = null;
            try
            {
                cfg = new ConfigReader();
                JvmVersionChecker checker = new JvmVersionChecker(cfg.MinJvmVersion, cfg.MaxJvmVersion);
                if (!checker.CheckAvaliableJvmInPath())
                {
                    if (cfg.MaxJvmVersion != null)
                        throw new JvmNotFoundException(cfg.MinJvmVersion, cfg.MaxJvmVersion);
                    else
                        throw new JvmNotFoundException(cfg.MinJvmVersion);
                }

                StartApp(cfg, args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, cfg?.Appname ?? ConfigReader.DEF_APP_NAME);

                if (e is JvmNotFoundException && cfg.JvmDlPath != "")
                    Process.Start(cfg.JvmDlPath);
            }
        }
    }
}
