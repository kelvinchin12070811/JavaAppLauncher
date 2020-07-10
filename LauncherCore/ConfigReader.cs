/************************************************************************************************************
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 ***********************************************************************************************************/
using YamlDotNet.Serialization;
using System.IO;

namespace LauncherCore
{
    class ConfigReader
    {
        public LauncherConfig Config { get; private set; } = null;
        public ConfigReader()
        {
            var path = "./io.gitlab.kelvinchin12070811/javaapplauncher/launcher.config.yaml";
            var cfgFile = new StreamReader(File.OpenRead(path));

            var decerealEngine = new DeserializerBuilder().Build();
            Config = decerealEngine.Deserialize<LauncherConfig>(cfgFile);
        }
    }
}
