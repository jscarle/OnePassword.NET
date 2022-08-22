namespace OnePassword.Common;

public class JsonWebKey
{
    [JsonPropertyName("alg")]
    public string Alg { get; set; } = "";

    [JsonPropertyName("ext")]
    public bool Extended { get; set; }

    [JsonPropertyName("k")]
    public string Key { get; set; } = "";

    [JsonPropertyName("kid")]
    public string KeyId { get; set; } = "";

    [JsonPropertyName("key_ops")]
    public List<string> KeyOperations { get; set; } = new();

    [JsonPropertyName("kty")]
    public string KeyType { get; set; } = "";

    [JsonPropertyName("use")]
    public string KeyUse { get; set; } = "";

    [JsonPropertyName("e")]
    public bool RsaExponent { get; set; }

    [JsonPropertyName("n")]
    public bool RsaModulus { get; set; }
}