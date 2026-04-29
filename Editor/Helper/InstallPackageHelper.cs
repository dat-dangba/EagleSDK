using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Eagle
{
    public static class InstallPackageHelper
    {
        private static Queue<string> packagesToInstall = new();
        private static AddRequest currentRequest;

        private static event Action OnInstallCompleted;

        public static bool IsPackageInstalled(string PackageId)
        {
            return Directory.Exists($"Packages/{PackageId}");
        }

        public static void Install(string package, Action completed = null)
        {
            OnInstallCompleted = completed;
            packagesToInstall = new Queue<string>();
            packagesToInstall.Enqueue(package);
            ProcessNextPackage();
        }

        public static void Install(List<string> packages, Action completed = null)
        {
            OnInstallCompleted = completed;
            packagesToInstall = new Queue<string>();
            foreach (var id in packages)
            {
                packagesToInstall.Enqueue(id);
            }

            ProcessNextPackage();
        }

        private static void ProcessNextPackage()
        {
            if (packagesToInstall.Count == 0)
            {
                EagleLog.Log("Tất cả package đã được cài đặt xong!", LogLevel.Verbose);
                OnInstallCompleted?.Invoke();
                OnInstallCompleted = null;
                return;
            }

            string nextPackage = packagesToInstall.Dequeue();
            EagleLog.Log($"Bắt đầu cài đặt {nextPackage}");
            currentRequest = Client.Add(nextPackage);

            EditorApplication.update += ProgressCheck;
        }

        private static void ProgressCheck()
        {
            if (!currentRequest.IsCompleted) return;

            EditorApplication.update -= ProgressCheck;

            if (currentRequest.Status == StatusCode.Success)
            {
                EagleLog.Log($"Đã cài xong {currentRequest.Result.packageId}");
            }
            else
            {
                EagleLog.LogError($"Lỗi khi cài {currentRequest.Error.message}");
            }

            ProcessNextPackage();
        }
    }
}