//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#pragma once
#include <boost/property_tree/ptree.hpp>
#include <string>
#include <sstream>
#include <vector>

class ConfigReader
{
public:
	static constexpr char DEF_APP_NAME[] = "Kelvin's Java App Launcher";
	static constexpr char LAUNCHER_CFG_FILE[] = "javaapplauncher.ini";

public:
	ConfigReader(const std::vector<std::string>* const args);

private:
	boost::property_tree::ptree parseLauncherCfg();
	std::stringstream getLauncherCfg();

public:// Accessors
	std::string getAppArgs() const;
	std::string getAppname() const;
	std::string getAppVersion() const;
	std::string getMaxJvmVersion() const;
	std::string getMinJvmversion() const;
	std::string getJarFilename() const;
	std::string getJvmArgs() const;
	std::string getJvmDlPath() const;
	std::string getJvmPath() const;

private: // Attributes
	std::string appArgs;
	std::string appname;
	std::string appVersion;
	std::string maxJvmVersion;
	std::string minJvmVersion;
	std::string jarFilename;
	std::string jvmArgs;
	std::string jvmDlPath;
	std::string jvmPath;

	const std::vector<std::string>* const args{ nullptr };
};