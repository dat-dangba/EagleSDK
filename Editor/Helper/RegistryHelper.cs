using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Eagle
{
    public static class RegistryHelper
    {
        public static void AddRegistryMAX()
        {
            var maxRegistry = new ScopedRegistry
            {
                name = "AppLovin MAX Unity",
                url = "https://unity.packages.applovin.com",
                scopes = new List<string>
                {
                    "com.applovin.mediation.ads",
                    "com.applovin.mediation.adapters",
                    "com.applovin.mediation.dsp"
                }
            };
            AddRegistry(maxRegistry);
        }
        
        public static void AddRegistryEDM4U()
        {
            var openUPM = new ScopedRegistry
            {
                name = "package.openupm.com",
                url = "https://package.openupm.com",
                scopes = new List<string> { EDM4UManager.PackageId }
            };
            AddRegistry(openUPM);
        }

        public static void AddRegistry(ScopedRegistry scopedRegistry)
        {
            string manifestPath = Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");
            if (!File.Exists(manifestPath))
            {
                EagleLog.Log($"Không tìm thấy file {manifestPath}");
                return;
            }

            string content = File.ReadAllText(manifestPath);
            if (content.Contains(scopedRegistry.name))
            {
                return;
            }

            UnityManifest manifest = JsonConvert.DeserializeObject<UnityManifest>(content);
            manifest.scopedRegistries ??= new List<ScopedRegistry>();
            manifest.scopedRegistries.Add(scopedRegistry);

            string newJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
            File.WriteAllText(manifestPath, newJson);

            EagleLog.Log($"Đã thêm {scopedRegistry.name} Registry thành công!");
            AssetDatabase.Refresh();
        }
    }
}