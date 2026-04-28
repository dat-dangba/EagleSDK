using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Eagle
{
    public static class EagleConfigService
    {
        private static Dictionary<Type, EagleEditorSettingBase> cacheConfigs = new();

        public static T GetConfig<T>() where T : EagleEditorSettingBase
        {
            if (cacheConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            // string path = $"{Constant.SettingsFolder}/{typeof(T).Name}.asset";
            // T t = AssetDatabase.LoadAssetAtPath<T>(path);
            string path = $"{Constant.SettingsFolder}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            cacheConfigs.Add(typeof(T), t);
            return t;
        }
    }
}