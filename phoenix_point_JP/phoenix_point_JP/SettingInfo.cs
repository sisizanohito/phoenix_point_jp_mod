using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace phoenix_point_JP
{
    public class SettingInfo
    {
        public string TargetDirectory { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string[] keys { get; set; }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
    string lpApplicationName,
    string lpKeyName,
    string lpDefault,
    StringBuilder lpReturnedstring,
    int nSize,
    string lpFileName);

        public string GetIniValue(string path, string section, string key)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, path);
            return sb.ToString();
        }
    }

}
