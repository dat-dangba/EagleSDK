using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Compilation;

namespace Eagle
{
    [InitializeOnLoad]
    public class SdkDetector
    {
        private static readonly Dictionary<string, string> SdkDefinitions = new()
        {
            { "HAS_MAX_SDK", "com.applovin.mediation.ads" },
            { "HAS_EAGLE_ANALYTICS", "com.eagle.analytics" },
            { "HAS_ADJUST_SDK", "com.adjust.sdk" },
        };

        static SdkDetector()
        {
            CompilationPipeline.compilationFinished += _ => UpdateDefineSymbolsAndReload();
        }

        private static void UpdateDefineSymbolsAndReload()
        {
            foreach (var item in SdkDefinitions)
            {
                bool isInstalled = IsPackageInstalled(item.Value);
                if (DefineSymbolIfNeeded(BuildTargetGroup.iOS, item.Key, isInstalled) |
                    DefineSymbolIfNeeded(BuildTargetGroup.Android, item.Key, isInstalled))
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

        private static bool IsPackageInstalled(string packageId)
        {
            return Directory.Exists($"Packages/{packageId}");
        }

        private static bool DefineSymbolIfNeeded(BuildTargetGroup targetGroup, string defineSymbol, bool isInstalled)
        {
            NamedBuildTarget namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out string[] currentDefineSymbols);
            if (!currentDefineSymbols.Contains(defineSymbol) && isInstalled)
            {
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget,
                    currentDefineSymbols.Append(defineSymbol).ToArray());
                return true;
            }

            if (currentDefineSymbols.Contains(defineSymbol) && !isInstalled)
            {
                PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget,
                    currentDefineSymbols.Except(new[] { defineSymbol }).ToArray());
                return true;
            }

            return false;
        }
    }
}