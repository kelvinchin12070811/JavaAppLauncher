using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Launcher
{
    class Launch
    {
        public static void StartApp()
        {
        }

        public static void Main(string[] args)
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

                Process jvm = new Process();
                jvm.StartInfo.FileName = cfg.GetJvmPath();
                jvm.StartInfo.UseShellExecute = false;
                jvm.StartInfo.Arguments = string.Format("{0} -jar \"{1}\"", cfg.JvmArgs, cfg.Filename);

                jvm.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Java app launcher");

                if (e is JvmNotFoundException && !cfg.JvmDlPath.Equals(""))
                    Process.Start(cfg.JvmDlPath);
            }
        }
    }
}
