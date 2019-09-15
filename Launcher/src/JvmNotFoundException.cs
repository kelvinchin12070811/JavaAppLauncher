//***********************************************************************************************************
//This Source Code Form is subject to the terms of the Mozilla Public
//License, v. 2.0. If a copy of the MPL was not distributed with this
//file, You can obtain one at http://mozilla.org/MPL/2.0/.
//***********************************************************************************************************
using System;

namespace Launcher
{
    class JvmNotFoundException : ApplicationException
    {
        public JvmNotFoundException(string minVersion):
            base(string.Format("This application require java version {0} or later.", minVersion))
        {
        }

        public JvmNotFoundException(string minVersion, string maxVersion):
            base(string.Format("This application require java version between {0}-{1}", minVersion,
                maxVersion))
        {
        }
    }
}
