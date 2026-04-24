#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace Eagle
{
    public static partial class GameConfig
    {
        [DllImport("__Internal")]
        private static extern IntPtr GetInfoPlistConfigValue(string key);

        private static string GetConfigValueIOS(string key)
        {
            IntPtr result = GetInfoPlistConfigValue(key);
            return result == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(result);
        }

        private static T GetConfigIOS<T>(string key, T defaultValue)
        {
            Type type = typeof(T);
            string value = GetConfigValueIOS(key);
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                if (type != typeof(bool)) return (T)Convert.ChangeType(value, type);

                if (value == "1" || value.ToLower() == "true") return (T)(object)true;
                if (value == "0" || value.ToLower() == "false") return (T)(object)false;

                return (T)Convert.ChangeType(value, type);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
#endif