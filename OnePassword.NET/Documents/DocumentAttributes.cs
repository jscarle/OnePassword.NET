using Newtonsoft.Json;
using OnePassword.Common;

namespace OnePassword.Documents
{
    public class DocumentAttributes
    {
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileName")]
        public long UnencryptedSize { get; set; }

        [JsonProperty("fileName")]
        public string Nonce { get; set; }

        [JsonProperty("fileName")]
        public JsonWebKey EncryptionKey { get; set; }

        [JsonProperty("fileName")]
        public long EncryptedSize { get; set; }

        [JsonProperty("signingKey")]
        public JsonWebKey SigningKey { get; set; }

        [JsonProperty("integrityHash")]
        public string IntegrityHash { get; set; }
    }
}
