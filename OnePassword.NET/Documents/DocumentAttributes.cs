using OnePassword.Common;

namespace OnePassword.Documents;

public class DocumentAttributes
{
    [JsonPropertyName("documentId")]
    public string DocumentId { get; set; } = "";

    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = "";

    [JsonPropertyName("fileName")]
    public long UnencryptedSize { get; set; }

    [JsonPropertyName("fileName")]
    public string Nonce { get; set; } = "";

    [JsonPropertyName("fileName")]
    public JsonWebKey EncryptionKey { get; set; } = new();

    [JsonPropertyName("fileName")]
    public long EncryptedSize { get; set; }

    [JsonPropertyName("signingKey")]
    public JsonWebKey SigningKey { get; set; } = new();

    [JsonPropertyName("integrityHash")]
    public string IntegrityHash { get; set; } = "";
}