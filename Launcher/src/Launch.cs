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
                reader.JvmArgs, reader.Filename, CatArgs(args));
            jvm.Start();
        }

        static string CatArgs(string[] args)
        {
            string line = "";

            foreach (var itr in args)
            {
                line += string.Format("\"{0}\" ", itr);
            }
            line = line.Substring(0, line.Length - 1);
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
                MessageBox.Show(e.Message, "Java app launcher");

                if (e is JvmNotFoundException && !cfg.JvmDlPath.Equals(""))
                    Process.Start(cfg.JvmDlPath);
            }
        }
    }
}
