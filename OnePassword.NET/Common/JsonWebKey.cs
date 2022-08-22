namespace OnePassword.Common;

public class JsonWebKey
{
    [JsonPropertyName("alg")]
    public string Alg { get; init; } = "";

    [JsonPropertyName("ext")]
    public bool Extended { get; init; }

    [JsonPropertyName("k")]
    public string Key { get; init; } = "";

    [JsonPropertyName("kid")]
    public string KeyId { get; init; } = "";

    [JsonPropertyName("key_ops")]
    public List<string> KeyOperations { get; init; } = new();

    [JsonPropertyName("kty")]
    public string KeyType { get; init; } = "";

    [JsonPropertyName("use")]
    public string KeyUse { get; init; } = "";

    [JsonPropertyName("e")]
    public bool RsaExponent { get; init; }

    [JsonPropertyName("n")]
    public bool RsaModulus { get; init; }
}