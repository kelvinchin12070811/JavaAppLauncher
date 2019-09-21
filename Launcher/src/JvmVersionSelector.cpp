//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#include <boost/process.hpp>
#include <regex>
#include "JvmVersionSelector.hpp"

JvmVersionSelector::JvmVersionSelector(const std::string& minJvmVersion, const std::string& maxJvmVersion):
	minJvmVersion(minJvmVersion), maxJvmVersion(maxJvmVersion)
{
	auto result = checkGlobalPathVersion();
}

std::filesystem::path JvmVersionSelector::getPrefferedJvmPath(const ConfigReader& reader)
{
	std::filesystem::path jvmPath{ reader.getJvmPath() };
	if (!jvmPath.empty())
		return jvmPath;

	return std::filesystem::path{};
}

bool JvmVersionSelector::checkGlobalPathVersion()
{
	namespace bp = boost::process;

	bp::ipstream output;
	std::regex versionPattern("version \"(.+?)\"");
	std::string versionLine;
	std::smatch matches;
	
	bp::system("javaw -version", bp::std_err > output);
	std::getline(output, versionLine);
	std::regex_search(versionLine, matches, versionPattern);
	if (matches.empty())
		return false;

	if (auto version = matches[1]; version >= minJvmVersion || version <= maxJvmVersion)
		return true;

	return false;
}
