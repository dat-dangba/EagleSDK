using System.IO;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Eagle
{
    public static class CreateProjectStructure
    {
        private const string PACKAGE_NAME = "com.eagle.basegame.v2";
        private const string SOURCE_FOLDER = "ProjectStructure~";

        public static void CopyProjectStructure()
        {
            var packageInfo = PackageInfo.FindForPackageName(PACKAGE_NAME);
            if (packageInfo == null)
            {
                Debug.LogError($"[ProjectStructure] Không tìm thấy package {PACKAGE_NAME}");
                return;
            }

            var sourcePath = Path.Combine(packageInfo.resolvedPath, SOURCE_FOLDER);
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError($"[ProjectStructure] Không tìm thấy folder: {sourcePath}");
                return;
            }

            CopyDirectory(sourcePath, Application.dataPath);
            AssetDatabase.Refresh();
            Debug.Log("[ProjectStructure] Copy Project Structure hoàn tất.");
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceDir, destDir));

            foreach (var filePath in Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                if (filePath.EndsWith(".meta") || filePath.EndsWith(".gitkeep"))
                    continue; // để Unity tự sinh meta mới, tránh trùng GUID giữa các project

                var destFile = filePath.Replace(sourceDir, destDir);
                if (!File.Exists(destFile))
                    File.Copy(filePath, destFile);
            }
        }
    }
}
