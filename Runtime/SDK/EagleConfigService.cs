using System;
using System.Collections.Generic;

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

#if UNITY_EDITOR
            string path = $"{Constant.SettingsFolder}/{typeof(T).Name}.asset";
            T t = AssetDatabase.LoadAssetAtPath<T>(path);
            cacheConfigs.Add(typeof(T), t);
            return t;
#else
            return null;
#endif
        }
    }
}