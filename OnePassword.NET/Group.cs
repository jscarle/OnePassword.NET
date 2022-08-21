using System;
using Newtonsoft.Json;
using OnePassword.Common;
using OnePassword.Groups;

namespace OnePassword
{
    public class Group
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public GroupType GroupType { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("permissions")]
        public long Permissions { get; set; }

        [JsonProperty("activeKeysetUuid")]
        public string ActiveKeysetUuid { get; set; }

        [JsonProperty("pubKey")]
        public JsonWebKey PublicKey { get; set; }

        [JsonProperty("attrVersion")]
        public int AttributesVersion { get; set; }
    }
}
