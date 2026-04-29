using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    public static class EagleServices
    {
        private static Dictionary<Type, EagleEditorSettingBase> editorSettings = new();
        private static Dictionary<Type, EagleBuildReflectionConfigBase> buildConfigs = new();

        public static T GetSetting<T>() where T : EagleEditorSettingBase
        {
            if (editorSettings.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.SettingsFolder}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            if (t != null && !editorSettings.ContainsKey(t.GetType()))
            {
                editorSettings.Add(t.GetType(), t);
            }

            return t;
        }

        public static T GetSetting<T>(string name) where T : EagleEditorSettingBase
        {
            if (editorSettings.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.SettingsFolder}/{name}";
            T t = Resources.Load<T>(path);
            if (t != null && !editorSettings.ContainsKey(t.GetType()))
            {
                editorSettings.Add(t.GetType(), t);
            }

            return t;
        }

        public static T GetBuildConfig<T>() where T : EagleBuildReflectionConfigBase
        {
            if (buildConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.BuildConfigFolder}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            if (t != null && !buildConfigs.ContainsKey(t.GetType()))
            {
                buildConfigs.Add(t.GetType(), t);
            }

            return t;
        }

        public static T GetBuildConfig<T>(string name) where T : EagleBuildReflectionConfigBase
        {
            if (buildConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.BuildConfigFolder}/{name}";
            T t = Resources.Load<T>(path);
            if (t != null && !buildConfigs.ContainsKey(t.GetType()))
            {
                buildConfigs.Add(t.GetType(), t);
            }

            return t;
        }
    }
}