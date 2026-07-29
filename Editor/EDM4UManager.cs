using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    public static class EDM4UManager
    {
        public const string PackageId = "com.google.external-dependency-manager";
        private const string Version = "1.2.178";
        private const string Tag = "[SetupEDM4U]";

        public static bool IsEDM4UInstalled()
        {
            return InstallPackageHelper.IsPackageInstalled(PackageId);
        }

        public static void InstallEDM4U()
        {
            RegistryHelper.AddRegistryEDM4U();
            Debug.Log($"{Tag} - Đang bắt đầu cài đặt EDM4U...");
            InstallPackageHelper.Install($"{PackageId}@{Version}");
        }
    }
}