//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#include <stdexcept>
#include <Windows.h>
#include <vector>
#include "ConfigReader.hpp"
#include "JvmVersionSelector.hpp"
#include "TextTools.hpp"

std::vector<std::string> parseCmdLine(LPWSTR cmdLine)
{
	std::vector<std::string> args;

	int argc{ 0 };
	wchar_t** argv{ CommandLineToArgvW(cmdLine, &argc) };
	args.reserve(cmdLine == L"" ? argc : argc + 1);

	if (cmdLine != L"")
	{
		wchar_t filename[MAX_PATH];
		GetModuleFileName(nullptr, filename, MAX_PATH);
		args.push_back(text_tools::wstos(filename));
	}
	
	for (int idx{ 0 }; idx < argc; idx++)
		args.push_back(text_tools::wstos(argv[idx]));

	LocalFree(argv);

	return args;
}

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR cmdline, int cmdShow)
{
	auto args = parseCmdLine(cmdline);
	std::unique_ptr<ConfigReader> reader{ nullptr };

	try
	{
		reader = std::make_unique<ConfigReader>(&args);
		JvmVersionSelector versionSelector{ reader->getMinJvmversion(), reader->getMaxJvmVersion() };
	}
	catch (std::exception& e)
	{
		MessageBeep(MB_ICONERROR);
		MessageBox(nullptr, text_tools::stows(e.what()).c_str(),
			text_tools::stows(reader ? reader->getAppname() : ConfigReader::DEF_APP_NAME).c_str(),
			MB_ICONERROR);
	}
	return 0;
}