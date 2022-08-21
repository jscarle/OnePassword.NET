using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnePassword.Common
{
    public class JsonWebKey
    {
        [JsonProperty("alg")]
        public string Alg { get; set; }

        [JsonProperty("ext")]
        public bool Extended { get; set; }

        [JsonProperty("k")]
        public string Key { get; set; }

        [JsonProperty("kid")]
        public string KeyId { get; set; }

        [JsonProperty("key_ops")]
        public List<string> KeyOperations { get; set; }

        [JsonProperty("kty")]
        public string KeyType { get; set; }

        [JsonProperty("use")]
        public string KeyUse { get; set; }

        [JsonProperty("e")]
        public bool RsaExponent { get; set; }

        [JsonProperty("n")]
        public bool RsaModulus { get; set; }

        public JsonWebKey()
        {
            KeyOperations = new List<string>();
        }
    }
}
