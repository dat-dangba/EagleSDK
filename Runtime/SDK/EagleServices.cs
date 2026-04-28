using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eagle
{
    public static class EagleServices
    {
        private static Dictionary<Type, EagleEditorSettingBase> cacheConfigs = new();

        public static T GetSetting<T>() where T : EagleEditorSettingBase
        {
            if (cacheConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.SettingsFolder}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            cacheConfigs.Add(typeof(T), t);
            return t;
        }
    }
}