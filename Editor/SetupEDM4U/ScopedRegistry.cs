using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eagle
{
    [Serializable]
    public class ScopedRegistry
    {
        [JsonProperty("name")] public string name { get; set; }

        [JsonProperty("url")] public string url { get; set; }

        [JsonProperty("scopes")] public List<string> scopes { get; set; }
    }
}