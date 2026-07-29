using System.IO;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    /// <summary>
    /// Editor tool để tạo project folder structure chuẩn cho _Project và _ThirdParty.
    /// Đặt file này vào Assets/Editor/ (không phải trong runtime folder).
    /// Sử dụng: Menu "Tools/Project Setup/Create Folder Structure"
    /// </summary>
    public static class CreateProjectStructure
    {
        // Danh sách các path folder cần tạo, tương đối so với Assets/
        private static readonly string[] FolderPaths = new[]
        {
            // _Project
            "_Project/Animations",
            "_Project/Audio",
            "_Project/Fonts",
            "_Project/Images",
            "_Project/Materials",
            "_Project/Model3D",
            "_Project/Prefabs/GamePlay",
            "_Project/Prefabs/UI",
            "_Project/Prefabs/VFX",
            "_Project/Scenes",
            "_Project/ScriptableObjects",
            "_Project/ScriptableObjects/_Scripts",
            "_Project/Scripts/Events",
            "_Project/Scripts/GamePlay/",
            "_Project/Scripts/UI",
            "_Project/Scripts/Enums",
            "_Project/Scripts/Interface",
            "_Project/Scripts/Utils",
            "_Project/Scripts/GameAction",
            "_Project/Settings",
            "_Project/Shaders",

            // _ThirdParty
            "_ThirdParty/",
        };

        // [MenuItem("Tools/Project Setup/Create Folder Structure")]
        public static void CreateFolders()
        {
            int createdCount = 0;
            int skippedCount = 0;

            foreach (string relativePath in FolderPaths)
            {
                string fullPath = Path.Combine(Application.dataPath, relativePath);

                if (Directory.Exists(fullPath))
                {
                    skippedCount++;
                    continue;
                }

                Directory.CreateDirectory(fullPath);
                createdCount++;

                // // Tạo .gitkeep để git track được folder rỗng
                // string gitkeepPath = Path.Combine(fullPath, ".gitkeep");
                // if (!File.Exists(gitkeepPath))
                // {
                //     File.WriteAllText(gitkeepPath, string.Empty);
                // }
            }

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Create Folder Structure",
                $"Đã tạo {createdCount} folder mới.\n{skippedCount} folder đã tồn tại từ trước.",
                "OK"
            );
        }

        // // Tuỳ chọn: xoá toàn bộ _Project và _ThirdParty (dùng thận trọng, chỉ cho project mới/test)
        // [MenuItem("Tools/Project Setup/Delete Folder Structure (Danger)")]
        // public static void DeleteFolders()
        // {
        //     bool confirm = EditorUtility.DisplayDialog(
        //         "Xoá Folder Structure",
        //         "Hành động này sẽ xoá _Project và _ThirdParty (nếu tồn tại) cùng toàn bộ nội dung bên trong. Bạn chắc chắn chứ?",
        //         "Xoá", "Huỷ"
        //     );
        //
        //     if (!confirm) return;
        //
        //     string[] rootFolders = { "_Project", "_ThirdParty" };
        //     foreach (string root in rootFolders)
        //     {
        //         string fullPath = Path.Combine(Application.dataPath, root);
        //         if (Directory.Exists(fullPath))
        //         {
        //             Directory.Delete(fullPath, true);
        //             string metaPath = fullPath + ".meta";
        //             if (File.Exists(metaPath)) File.Delete(metaPath);
        //         }
        //     }
        //
        //     AssetDatabase.Refresh();
        //     Debug.Log("[CreateProjectStructure] Đã xoá _Project và _ThirdParty.");
        // }
    }
}