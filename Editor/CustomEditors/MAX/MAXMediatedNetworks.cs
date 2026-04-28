using System.Collections.Generic;
using UnityEngine;

namespace Eagle
{
    [System.Serializable]
    public struct MediatedNetwork
    {
        public string Name;
        public string Package;
        public string AndroidVersion;
        public string IOSVersion;

        public MediatedNetwork(string name, string package, string androidVersion, string iosVersion)
        {
            Name = name;
            Package = package;
            AndroidVersion = androidVersion;
            IOSVersion = iosVersion;
        }
    }

    public class MAXMediatedNetworks : ScriptableObject
    {
        public List<MediatedNetwork> MediatedNetworks = new();

        public List<string> GetAllPackages()
        {
            var packages = new List<string>();
            foreach (var item in MediatedNetworks)
            {
                packages.Add($"com.applovin.mediation.adapters.{item.Package}.android@{item.AndroidVersion}");
                packages.Add($"com.applovin.mediation.adapters.{item.Package}.ios@{item.IOSVersion}");
            }

            return packages;
        }

        [ContextMenu("Create Mediated Networks")]
        public void CreateMediatedNetworks()
        {
            MediatedNetworks = new List<MediatedNetwork>
            {
                new("BidMachine", "bidmachine", "3060100.0.0", "306010000.0.0"),
                new("Chartboost", "chartboost", "9110100.0.0", "9110000.0.0"),
                new("DT Exchange", "fyber", "8040400.0.0", "8040600.0.0"),
                new("Facebook", "facebook", "6210000.0.0", "6210100.0.0"),
                new("Google", "google", "25020000.0.0", "13030000.0.0"),
                new("InMobi", "inmobi", "11020000.0.0", "11020000.0.0"),
                new("ironSource", "ironsource", "904000000.0.0", "904000000.0.0"),
                new("Liftoff", "vungle", "7070200.0.0", "7070200.0.0"),
                new("Mintegral", "mintegral", "17015100.0.0", "801010000.0.0"),
                new("Pangle", "bytedance", "709010300.0.0", "709010200.0.0"),
                new("Unity Ads", "unityads", "4170000.0.0", "4170000.0.0"),
                new("Verve", "verve", "3080100.0.0", "3080000.0.0"),
                new("Yandex", "yandex", "7180500.0.0", "7180400.0.0"),
            };
        }
    }
}