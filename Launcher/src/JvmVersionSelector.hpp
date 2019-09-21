//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#pragma once
#include <filesystem>
#include <string>
#include "ConfigReader.hpp"

class JvmVersionSelector
{
public:
	JvmVersionSelector(const std::string& minJvmVersion, const std::string& maxJvmVersion = "");

	std::filesystem::path getPrefferedJvmPath(const ConfigReader& reader);
private:
	bool checkGlobalPathVersion();
private:
	std::string minJvmVersion;
	std::string maxJvmVersion;
};