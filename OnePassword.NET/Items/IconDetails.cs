using OnePassword.Common;

namespace OnePassword.Items;

public class IconDetails
{
    [JsonPropertyName("fileId")]
    public string FileId { get; set; } = "";

    [JsonPropertyName("nonce")]
    public string Nonce { get; set; } = "";

    [JsonPropertyName("encryptionKey")]
    public JsonWebKey EncryptionKey { get; set; } = new();

    [JsonPropertyName("signingKey")]
    public JsonWebKey SigningKey { get; set; } = new();
}