using OnePassword.Common;

namespace OnePassword.Items;

public class IconDetails
{
    [JsonPropertyName("fileId")]
    public string FileId { get; init; } = "";

    [JsonPropertyName("nonce")]
    public string Nonce { get; init; } = "";

    [JsonPropertyName("encryptionKey")]
    public JsonWebKey EncryptionKey { get; init; } = new();

    [JsonPropertyName("signingKey")]
    public JsonWebKey SigningKey { get; init; } = new();
}