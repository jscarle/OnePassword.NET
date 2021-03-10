using Newtonsoft.Json;
using OnePassword.Common;

namespace OnePassword.Items
{
    public class IconDetails
    {
        [JsonProperty("fileId")]
        public string FileID { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("encryptionKey")]
        public JsonWebKey EncryptionKey { get; set; }

        [JsonProperty("signingKey")]
        public JsonWebKey SigningKey { get; set; }
    }
}
