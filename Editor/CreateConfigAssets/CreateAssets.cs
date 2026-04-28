#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    [InitializeOnLoad]
    public static class CreateAssets
    {
        static CreateAssets()
        {
            CreateAsset<GeneralSetting>(Constant.SettingsFolder);
            CreateAsset<MAXSetting>(Constant.SettingsFolder);
        }

        public static T CreateAsset<T>(string folderPath) where T : ScriptableObject
        {
            EnsureFolderExists(folderPath);

            string configName = typeof(T).Name;
            string fullPath = Path.Combine(folderPath, $"{configName}.asset");
            if (File.Exists(fullPath)) return null;
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"EagleSDK - Create {configName}: {fullPath}");
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
#endif