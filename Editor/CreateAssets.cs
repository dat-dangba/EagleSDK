using System.IO;
using UnityEditor;
using UnityEngine;
#if HAS_ADJUST_SDK
using AdjustSdk;
#endif


namespace Eagle
{
    [InitializeOnLoad]
    public static class CreateAssets
    {
        static CreateAssets()
        {
#if HAS_ADJUST_SDK
            AdjustSettings adjustSettings = AdjustSettings.Instance;
            AdjustSettings.iOSFrameworkAdSupport = true;
            AdjustSettings.iOSFrameworkAdServices = true;
            AdjustSettings.iOSFrameworkAdServices = true;
            AdjustSettings.iOSFrameworkAppTrackingTransparency = true;
            AdjustSettings.iOSFrameworkStoreKit = true;
#endif
#if HAS_MAX_SDK
            CreateAsset<MAXSetting>(Constant.SettingsFolder);
#endif
            CreateAsset<GeneralSetting>(Constant.SettingsFolder);
            CreateAsset<AdjustSetting>(Constant.SettingsFolder);
            CreateAsset<BaseGameSetting>(Constant.SettingsFolder);
        }

        public static T CreateAsset<T>(string folderPath) where T : ScriptableObject
        {
            string path = $"Assets/Resources/{folderPath}";

            EnsureFolderExists(path);

            string configName = typeof(T).Name;
            string fullPath = Path.Combine(path, $"{configName}.asset");
            if (File.Exists(fullPath)) return null;
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            EagleLog.Log($"Create {configName}: {fullPath}", LogLevel.Verbose);
            return asset as T;
        }

        private static void EnsureFolderExists(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;

            if (Directory.Exists(folderPath)) return;

            Directory.CreateDirectory(folderPath);
        }
    }
}