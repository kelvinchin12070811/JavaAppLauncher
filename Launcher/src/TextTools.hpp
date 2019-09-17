//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#pragma once
#include <string>

#if defined(_DEBUG) && defined(_MSC_VER)
#include <boost/lexical_cast.hpp>
#define DEBUG_LOG(str) text_tools::inner::__debug_log(str)
#else
#define DEBUG_LOG(str)
#endif

namespace text_tools
{
	std::string wstos(const std::wstring& input, bool isutf8 = true);
	std::wstring stows(const std::string& input, bool isutf8 = true);

#if defined(_DEBUG) && defined(_MSC_VER)
	namespace inner
	{
		inline void __debug_log(const std::string& value)
		{
			OutputDebugStringW(text_tools::stows(value + "\n").c_str());
		}

		template <typename T>
		inline void __debug_log(const T& value)
		{
			__debug_log(boost::lexical_cast<std::string>(value));
		}
	}
#endif
}