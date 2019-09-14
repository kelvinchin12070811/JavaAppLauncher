using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
