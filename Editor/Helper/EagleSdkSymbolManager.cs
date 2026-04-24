using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Eagle
{
    [InitializeOnLoad]
    public class EagleSdkSymbolManager
    {
        // Danh sách các SDK cần quản lý. Bạn có thể thêm các SDK khác vào đây.
        private static readonly Dictionary<string, string> SdkDefinitions = new()
        {
            { "HAS_MAX_SDK", "Packages/com.applovin.mediation.ads" },
            // { "HAS_ADJUST_SDK", "Packages/com.adjust.sdk" },
            // { "HAS_FIREBASE_ANALYTICS", "Packages/com.google.firebase.analytics" }
        };

        static EagleSdkSymbolManager()
        {
            // RefreshSymbols();
        }

        private static void RefreshSymbols()
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (targetGroup == BuildTargetGroup.Unknown) return;

            // string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            string currentDefines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            HashSet<string> symbolList = new HashSet<string>(currentDefines.Split(';').Select(s => s.Trim()));

            bool changed = false;

            foreach (var sdk in SdkDefinitions)
            {
                string symbol = sdk.Key;
                string path = sdk.Value;

                // Kiểm tra sự tồn tại của SDK (trong Packages hoặc Assets)
                bool isInstalled = Directory.Exists(path) || Directory.Exists(path.Replace("Packages/", "Assets/"));

                if (isInstalled && !symbolList.Contains(symbol))
                {
                    symbolList.Add(symbol);
                    changed = true;
                    Debug.Log($"<b>[EagleSDK]</b> <color=#00FF00>Enabled</color> symbol: {symbol}");
                }
                else if (!isInstalled && symbolList.Contains(symbol))
                {
                    symbolList.Remove(symbol);
                    changed = true;
                    Debug.Log($"<b>[EagleSDK]</b> <color=#FF0000>Disabled</color> symbol: {symbol}");
                }
            }

            if (!changed) return;
            string newDefines = string.Join(";", symbolList.ToArray());
            // PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefines);
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newDefines);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}