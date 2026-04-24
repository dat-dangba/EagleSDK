#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    [InitializeOnLoad]
    public static class CreateConfigAssets
    {
        static CreateConfigAssets()
        {
            CreateFolder();

            // CreateBuildConfig<EagleAdjustConfig>();

            CreateEditorConfig<GeneralConfig>();
            CreateEditorConfig<MAXConfig>();
        }

        private static void CreateBuildConfig<T>() where T : ScriptableObject
        {
            CreateConfig<T>($"{Constant.EagleSDKFolder}/{Constant.BuildConfigFolder}");
        }

        private static void CreateEditorConfig<T>() where T : ScriptableObject
        {
            CreateConfig<T>($"{Constant.EagleSDKFolder}/{Constant.ConfigFolder}");
        }

        private static void CreateConfig<T>(string folderPath) where T : ScriptableObject
        {
            string configName = typeof(T).Name;
            string fullPath = Path.Combine($"{Constant.ResourcePath}/{folderPath}", $"{configName}.asset");
            if (File.Exists(fullPath)) return;
            Debug.Log($"EagleSDK - Create Config {configName}\n{fullPath}");
            ScriptableObject asset = ScriptableObject.CreateInstance(typeof(T));
            AssetDatabase.CreateAsset(asset, fullPath);
            AssetDatabase.SaveAssets();
        }

        private static void CreateFolder()
        {
            if (!AssetDatabase.IsValidFolder(Constant.ResourcePath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string eagleSDKPath = $"{Constant.ResourcePath}/{Constant.EagleSDKFolder}";
            if (!AssetDatabase.IsValidFolder(eagleSDKPath))
            {
                AssetDatabase.CreateFolder(Constant.ResourcePath, Constant.EagleSDKFolder);
            }

            string buildConfigPath = $"{eagleSDKPath}/{Constant.BuildConfigFolder}";
            if (!AssetDatabase.IsValidFolder(buildConfigPath))
            {
                AssetDatabase.CreateFolder(eagleSDKPath, Constant.BuildConfigFolder);
            }

            string editorConfigPath = $"{eagleSDKPath}/{Constant.ConfigFolder}";
            if (!AssetDatabase.IsValidFolder(editorConfigPath))
            {
                AssetDatabase.CreateFolder(eagleSDKPath, Constant.ConfigFolder);
            }
        }
    }
}
#endif