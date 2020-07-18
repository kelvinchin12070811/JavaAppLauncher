/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using YamlDotNet.Serialization;
using System.IO;
using System.Reflection;

namespace LauncherCore
{
    class ConfigReader
    {
        public LauncherConfig Config { get; private set; } = null;
        public ConfigReader()
        {
            var pwd = Assembly.GetEntryAssembly().Location;
            pwd = Path.GetDirectoryName(pwd);
            var path = $"{pwd}//javaapplauncher.cfg";
            var cfgFile = new StreamReader(File.OpenRead(path));

            var decerealEngine = new DeserializerBuilder().Build();
            Config = decerealEngine.Deserialize<LauncherConfig>(cfgFile);
            cfgFile.Close();
        }
    }
}
