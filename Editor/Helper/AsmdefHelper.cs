using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Eagle
{
    public static class AsmdefHelper
    {
        private const string AsmdefPath = "Assets/EagleSDK/Runtime/EagleSDK.asmdef";

        public static void AddAssemblyReference(string assemblyName)
        {
            if (!File.Exists(AsmdefPath)) return;

            string json = File.ReadAllText(AsmdefPath);
            // Sử dụng một class hỗ trợ để parse Json (Unity có thể dùng JsonUtility)
            AsmdefData data = JsonUtility.FromJson<AsmdefData>(json);

            if (data.references == null) data.references = new string[0];

            if (!data.references.Contains(assemblyName))
            {
                var refList = data.references.ToList();
                refList.Add(assemblyName);
                data.references = refList.ToArray();

                string newJson = JsonUtility.ToJson(data, true);
                File.WriteAllText(AsmdefPath, newJson);

                AssetDatabase.Refresh();
                Debug.Log($"<b>[EagleSDK]</b> Đã thêm Assembly Reference: {assemblyName}");
            }
        }

        [System.Serializable]
        private class AsmdefData
        {
            public string name;
            public string[] references;
            public string[] includePlatforms;
            public string[] excludePlatforms;
        }
    }
}