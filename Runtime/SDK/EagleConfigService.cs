using System;
using System.Collections.Generic;
using UnityEngine;

namespace Eagle
{
    public static class EagleConfigService
    {
        private static Dictionary<Type, EagleEditorConfigBase> cacheConfigs = new();

        public static T GetConfig<T>() where T : EagleEditorConfigBase
        {
            if (cacheConfigs.TryGetValue(typeof(T), out var config))
            {
                return config as T;
            }

            string path = $"{Constant.EagleSDKFolder}/{Constant.ConfigFolder}/{typeof(T).Name}";
            T t = Resources.Load<T>(path);
            cacheConfigs.Add(typeof(T), t);
            return t;
        }
    }
}