using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LauncherCore
{
    /// <summary>
    /// Information about the JVM.
    /// </summary>
    public class JVMInfo
    {
        /// <summary>
        /// Determine if the JVM exist on client.
        /// </summary>
        public bool Exist = false;
        /// <summary>
        /// Determine the path of the JVM's JavaHome path.
        /// </summary>
        public string Path = null;
        /// <summary>
        /// Determine the version of the JVM, formated in x.y.z even with java 8 or bellow.
        /// </summary>
        public JVMVesion Version = null;
    }
}
