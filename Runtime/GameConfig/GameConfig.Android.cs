#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eagle
{
    public static partial class GameConfig
    {
        private static AndroidJavaObject metaData;

        private static readonly Dictionary<Type, string> methodMap = new()
        {
            { typeof(int), "getInt" },
            { typeof(float), "getFloat" },
            { typeof(string), "getString" },
            { typeof(bool), "getBoolean" }
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            metaData = GetMetaData();
        }

        private static AndroidJavaObject GetMetaData()
        {
            try
            {
                AndroidJavaClass unityPlayer = new("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                string packageName = currentActivity.Call<string>("getPackageName");
                AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
                AndroidJavaObject applicationInfo =
                    packageManager.Call<AndroidJavaObject>("getApplicationInfo", packageName, 128);
                return applicationInfo.Get<AndroidJavaObject>("metaData");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool ContainsKey(string key)
        {
            return metaData.Call<bool>("containsKey", key);
        }

        private static T GetConfigAndroid<T>(string key, T defaultValue)
        {
            Type type = typeof(T);
            try
            {
                if (ContainsKey(key) && methodMap.TryGetValue(type, out string methodName))
                {
                    return metaData.Call<T>(methodName, key);
                }
            }
            catch (Exception)
            {
                return defaultValue;
            }

            return defaultValue;
        }
    }
}
#endif