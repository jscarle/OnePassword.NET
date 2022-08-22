using OnePassword.Common;

namespace OnePassword.Documents;

public class DocumentAttributes
{
    [JsonPropertyName("documentId")]
    public string DocumentId { get; init; } = "";

    [JsonPropertyName("fileName")]
    public string FileName { get; init; } = "";

    [JsonPropertyName("fileName")]
    public long UnencryptedSize { get; init; }

    [JsonPropertyName("fileName")]
    public string Nonce { get; init; } = "";

    [JsonPropertyName("fileName")]
    public JsonWebKey EncryptionKey { get; init; } = new();

    [JsonPropertyName("fileName")]
    public long EncryptedSize { get; init; }

    [JsonPropertyName("signingKey")]
    public JsonWebKey SigningKey { get; init; } = new();

    [JsonPropertyName("integrityHash")]
    public string IntegrityHash { get; init; } = "";
}