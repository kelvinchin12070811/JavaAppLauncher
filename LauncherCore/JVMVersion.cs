using System;
using System.Text.RegularExpressions;

namespace LauncherCore
{
    public class JVMVersion
    {
        public static readonly JVMVersion EMPTY = new JVMVersion();
        public int Feature { get; private set; } = 0;
        public int Interim { get; private set; } = 0;
        public int Update { get; private set; } = 0;
        public int Patch { get; private set; } = 0;

        public JVMVersion(int feature = 0, int interim = 0, int update = 0, int patch = 0)
        {
            Feature = feature;
            Interim = interim;
            Update = update;
            Patch = patch;
        }

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

        private void VersionParser(string version)
        {
            if (version == string.Empty) return;

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
                return;
            }

            var splited = version.Split('.');
            if (splited.Length != 1)
                Feature = int.Parse(splited[1]);
            else
                Feature = int.Parse(splited[0]);
        }

        public static explicit operator JVMVersion(string obj)
        {
            return new JVMVersion(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
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
            if (Equals(lhs, null))
                return Equals(rhs, null) ? true : false;
            else
                return lhs.Equals(rhs);
        }

        public static bool operator!=(JVMVersion lhs, JVMVersion rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator<(JVMVersion lhs, JVMVersion rhs)
        {
            if (lhs.Feature > rhs.Feature)
            {
                return false;
            }
            else if (lhs.Feature == rhs.Feature)
            {
                if (lhs.Interim > rhs.Interim)
                {
                    return false;
                }
                else if (lhs.Interim == rhs.Interim)
                {
                    if (lhs.Update > rhs.Update)
                    {
                        return false;
                    }
                    else if (lhs.Update == rhs.Update)
                    {
                        if (lhs.Patch >= rhs.Patch)
                            return false;
                    }
                }
            }

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
