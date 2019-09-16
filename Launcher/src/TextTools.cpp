//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#include <memory>
#include <Windows.h>
#include "TextTools.hpp"

namespace text_tools
{
	std::string wstos(const std::wstring& input, bool isutf8)
	{
		if (input.empty()) return std::string{};

		auto charCodePage{ isutf8 ? CP_UTF8 : CP_ACP };

		size_t length = WideCharToMultiByte(charCodePage, 0, input.data(), input.length() + 1, nullptr, 0, 0,
			FALSE);
		std::unique_ptr<char[]> rawStr = std::make_unique<char[]>(length);
		WideCharToMultiByte(charCodePage, 0, input.data(), input.length() + 1, rawStr.get(), length, 0,
			FALSE);
		return std::string{ rawStr.get() };
	}
	
	std::wstring stows(const std::string& input, bool isutf8)
	{
		if (input.empty()) return std::wstring{};
		
		auto charCodePage{ isutf8 ? CP_UTF8 : CP_ACP };

		size_t length = MultiByteToWideChar(charCodePage, 0, input.data(), input.length() + 1, nullptr, 0);
		std::unique_ptr<wchar_t[]> rawStr = std::make_unique<wchar_t[]>(length);
		MultiByteToWideChar(charCodePage, 0, input.data(), input.length() + 1, rawStr.get(), length);
		return std::wstring{ rawStr.get() };
	}
}