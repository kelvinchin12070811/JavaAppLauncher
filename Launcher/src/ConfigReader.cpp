//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#include <boost/property_tree/ini_parser.hpp>
#include <filesystem>
#include <fstream>
#include <iomanip>
#include <ZipLib/ZipArchive.h>
#include "ConfigReader.hpp"
#include "TextTools.hpp"

ConfigReader::ConfigReader(const std::vector<std::string>* const args):
	args(args)
{
	std::filesystem::path exeFile{ (*args)[0] };
	jarFilename = exeFile.replace_extension(".jar").string();

	std::wifstream fs;
	fs.open(text_tools::stows(jarFilename));
	if (!fs.is_open())
	{
		jarFilename = exeFile.replace_extension(".exe").string();
	}

	auto launcherCfg = parseLauncherCfg();
	MessageBox(nullptr, text_tools::stows(launcherCfg.get<std::string>("app.name")).c_str(), L"NO", MB_ICONINFORMATION);
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
	using namespace std::string_literals;
	ZipArchive::Ptr jarFile{ nullptr };
	std::ifstream reader;
	reader.open(text_tools::stows(jarFilename), std::ios::binary);
	if (!reader.is_open())
		throw std::runtime_error{ "Error on openning " + jarFilename };

	reader.seekg(0, std::ifstream::end);

	jarFile = ZipArchive::Create(&reader, false);

	auto cfgFile = jarFile->GetEntry(LAUNCHER_CFG_FILE);
	if (!cfgFile)
		throw std::runtime_error{ "Error to find "s + LAUNCHER_CFG_FILE + " in " + jarFilename };

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
