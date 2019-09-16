//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
#pragma once
#include <string>

namespace text_tools
{
	std::string wstos(const std::wstring& input, bool isutf8 = true);
	std::wstring stows(const std::string& input, bool isutf8 = true);
}