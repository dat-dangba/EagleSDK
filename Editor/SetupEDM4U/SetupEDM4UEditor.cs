#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Eagle
{
    [InitializeOnLoad]
    public static class SetupEDM4UEditor
    {
        private const string PackageId = "com.google.external-dependency-manager";
        private const string Version = "1.2.178";
        private const string Tag = "[SetupEDM4U]";

        static SetupEDM4UEditor()
        {
            if (IsEDM4UInstalled()) return;
            Debug.Log($"{Tag} - Project chưa được cài đặt 'External Dependency Manager' (EDM4U)");
            AddRegistry();
            ShowDialogSetupEDM4U();
        }

        private static void AddRegistry()
        {
            var openUPM = new ScopedRegistry
            {
                name = "package.openupm.com",
                url = "https://package.openupm.com",
                scopes = new List<string> { PackageId }
            };
            RegistryHelper.AddRegistry(openUPM);
        }

        private static bool IsEDM4UInstalled()
        {
            return Directory.Exists($"Packages/{PackageId}");
        }

        private static void ShowDialogSetupEDM4U()
        {
            bool userConfirm = EditorUtility.DisplayDialog(
                "EagleSDK Dependency Check",
                "EagleSDK cần package 'External Dependency Manager' (EDM4U) để hoạt động. Bạn có muốn cài đặt nó ngay không?",
                "Cài đặt ngay"
            );
            if (userConfirm)
            {
                InstallEDM4U();
            }
        }

        private static void InstallEDM4U()
        {
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt EDM4U...");
            InstallPackageHelper.Install($"{PackageId}@{Version}");
        }
    }
}
#endif