using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    public static class EDM4UManager
    {
        private const string PackageId = "com.google.external-dependency-manager";
        private const string Version = "1.2.178";
        private const string Tag = "[SetupEDM4U]";

        // static SetupEDM4UEditor()
        // {
        //     if (IsEDM4UInstalled()) return;
        //     Debug.Log($"{Tag} - Project chưa được cài đặt 'External Dependency Manager' (EDM4U)");
        //     AddRegistry();
        //     ShowDialogSetupEDM4U();
        // }

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

        public static bool IsEDM4UInstalled()
        {
            return InstallPackageHelper.IsPackageInstalled(PackageId);
        }

        public static void InstallEDM4U()
        {
            AddRegistry();
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt EDM4U...");
            InstallPackageHelper.Install($"{PackageId}@{Version}");
        }
    }
}