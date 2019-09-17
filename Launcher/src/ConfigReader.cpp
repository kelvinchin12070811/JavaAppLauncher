//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#include <boost/property_tree/ini_parser.hpp>
#include <filesystem>
#include <ZipLib/ZipArchive.h>
#include <ZipLib/ZipFile.h>
#include "ConfigReader.hpp"
#include "TextTools.hpp"

ConfigReader::ConfigReader(const std::vector<std::string>* const args):
	args(args)
{
	jarFilename = "DemoApp.jar";
	parseLauncherCfg();
}

boost::property_tree::ptree ConfigReader::parseLauncherCfg()
{
	boost::property_tree::ptree tree;
	auto ss = getLauncherCfg();
	boost::property_tree::ini_parser::read_ini(ss, tree);
	return tree;
}

std::stringstream ConfigReader::getLauncherCfg()
{
	ZipArchive::Ptr jarArhive = ZipFile::Open(jarFilename);
	auto cfgFile = jarArhive->GetEntry(ConfigReader::LAUNCHER_CFG_FILE);
	if (cfgFile == nullptr)
	{
		using namespace std;
		throw runtime_error{ "Failed to locate "s + ConfigReader::LAUNCHER_CFG_FILE + " at "
			+ jarFilename };
	}

	std::stringstream ss;
	ss << cfgFile->GetDecompressionStream()->rdbuf();
	return ss;
}

std::string ConfigReader::getAppname() const
{
	return appname.empty() ? DEF_APP_NAME : appname;
}

std::string ConfigReader::getMaxJvmVersion() const
{
	return maxJvmVersion;
}

std::string ConfigReader::getMinJvmversion() const
{
	return minJvmVersion;
}

std::string ConfigReader::getJarFilename() const
{
	return jarFilename;
}

std::string ConfigReader::getJvmArgs() const
{
	return jvmArgs;
}

std::string ConfigReader::getJvmDlPath() const
{
	return jvmDlPath;
}

std::string ConfigReader::getJvmPath() const
{
	if (!jvmPath.empty()) return jvmPath;

	return "javaw.exe";
}
