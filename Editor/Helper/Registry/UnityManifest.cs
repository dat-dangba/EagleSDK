using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eagle
{
    [Serializable]
    public class UnityManifest
    {
        [JsonProperty("scopedRegistries")] public List<ScopedRegistry> scopedRegistries { get; set; }

        [JsonProperty("dependencies")] public Dictionary<string, string> dependencies { get; set; }
    }
}