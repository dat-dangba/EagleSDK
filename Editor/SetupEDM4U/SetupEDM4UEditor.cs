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

        private static AddRequest addRequest;

        static SetupEDM4UEditor()
        {
            if (IsEDM4UInstalled()) return;
            Debug.Log($"{Tag} - Project chưa được cài đặt 'External Dependency Manager' (EDM4U)");
            AddRegistry();
            ShowDialogSetupEDM4U();
        }

        private static void AddRegistry()
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                Debug.Log($"{Tag} - Không tìm thấy file {manifestPath}");
                return;
            }

            string content = File.ReadAllText(manifestPath);
            if (content.Contains(PackageId))
            {
                return;
            }

            UnityManifest manifest = JsonConvert.DeserializeObject<UnityManifest>(content);
            manifest.scopedRegistries ??= new List<ScopedRegistry>();

            var openUPM = new ScopedRegistry
            {
                name = "package.openupm.com",
                url = "https://package.openupm.com",
                scopes = new List<string> { PackageId }
            };

            manifest.scopedRegistries.Add(openUPM);

            string newJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(manifestPath, newJson);

            Debug.Log($"{Tag} - Đã thêm OpenUPM Registry thành công!");
            AssetDatabase.Refresh();
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
                InstallPackage();
            }
        }

        private static void InstallPackage()
        {
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt EDM4U...");
            addRequest = Client.Add($"{PackageId}@{Version}");
            EditorApplication.update += Progress;
        }

        private static void Progress()
        {
            if (!addRequest.IsCompleted) return;

            switch (addRequest.Status)
            {
                case StatusCode.Success:
                    Debug.Log($"{Tag} - Cài đặt EDM4U thành công: " + addRequest.Result.packageId);
                    break;
                case >= StatusCode.Failure:
                    Debug.LogError($"{Tag} - Cài đặt EDM4U thất bại: " + addRequest.Error.message);
                    break;
            }

            EditorApplication.update -= Progress;
        }
    }
}
#endif