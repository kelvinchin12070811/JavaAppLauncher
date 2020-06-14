using System;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    /// <summary>
    /// Class that represent the version of JVM.
    /// See official documentation of Version-String for detailed reference.
    /// https://docs.oracle.com/javase/10/install/version-string-format.htm#JSJIG-GUID-DCA60310-6565-4BB6-8D24-6FF07C1C4B4E
    /// </summary>
    public class JVMVersion
    {
        public int Feature { get; private set; } = 0;
        public int Interim { get; private set; } = 0;
        public int Update { get; private set; } = 0;
        public int Patch { get; private set; } = 0;

        /// <summary>
        /// Create JVMVersion from seperated data.
        /// </summary>
        /// <param name="feature">Feature section of JVM Version.</param>
        /// <param name="interim">Interim section of JVM Version.</param>
        /// <param name="update">Update section of JVM Version.</param>
        /// <param name="patch">Patch section of JVM Version.</param>
        public JVMVersion(int feature = 0, int interim = 0, int update = 0, int patch = 0)
        {
            Feature = feature;
            Interim = interim;
            Update = update;
            Patch = patch;
        }

        /// <summary>
        /// Create JVMVersion from Version-String.
        /// </summary>
        /// <param name="version">Version-String.</param>
        public JVMVersion(string version)
        {
            VersionParser(version);
        }

        public override string ToString()
        {
            if (Patch != 0)
                return string.Format("{0}.{1}.{2}.{3}", Feature, Interim, Update, Patch);

            return string.Format("{0}.{1}.{2}", Feature, Interim, Update);
        }

        /// <summary>
        /// Parse JVM version to latest syntax with tech spec.
        /// </summary>
        /// <param name="version">Version number to parse</param>
        private void VersionParser(string version)
        {
            var java8VersionFormat = new Regex("^1\\.(\\d+)\\.(\\d+)_(\\d+)");
            var matchResult = java8VersionFormat.Match(version);

            if (!matchResult.Success)
            {
                var java9VersionFormat = new Regex("^(\\d+)\\.(\\d+)\\.(\\d+)(\\.(\\d+))?");
                matchResult = java9VersionFormat.Match(version);
            }

            if (matchResult.Success)
            {
                var matchGroups = matchResult.Groups;
                Feature = int.Parse(matchGroups[1].Value);
                Interim = int.Parse(matchGroups[2].Value);
                Update = int.Parse(matchGroups[3].Value);
                if (matchGroups[5].Value != "")
                    Patch = int.Parse(matchGroups[5].Value);
            }
        }

        public override bool Equals(object obj)
        {
            JVMVersion rhs = (JVMVersion)obj;
            return (Feature == rhs.Feature) && (Interim == rhs.Interim) &&
                (Update == rhs.Update) && (Patch == rhs.Patch);
        }

        public override int GetHashCode()
        {
            int hashCode = 24224089;
            hashCode = hashCode * -1521134295 + Feature.GetHashCode();
            hashCode = hashCode * -1521134295 + Interim.GetHashCode();
            hashCode = hashCode * -1521134295 + Update.GetHashCode();
            hashCode = hashCode * -1521134295 + Patch.GetHashCode();
            return hashCode;
        }

        public static bool operator==(JVMVersion lhs, JVMVersion rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator!=(JVMVersion lhs, JVMVersion rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator<(JVMVersion lhs, JVMVersion rhs)
        {
            if (lhs.Feature >= rhs.Feature) return false;
            else if (lhs.Interim >= rhs.Interim) return false;
            else if (lhs.Update >= rhs.Update) return false;
            else if (lhs.Patch >= rhs.Patch) return false;

            return true;
        }

        public static bool operator>(JVMVersion lhs, JVMVersion rhs)
        {
            return !(lhs < rhs) && (lhs != rhs);
        }

        public static bool operator<=(JVMVersion lhs, JVMVersion rhs)
        {
            return !(lhs > rhs);
        }

        public static bool operator >=(JVMVersion lhs, JVMVersion rhs)
        {
            return !(lhs < rhs);
        }
    }
}
