using OnePassword.Common;

namespace OnePassword.Groups;

public class Group
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("desc")]
    public string Description { get; set; } = "";

    [JsonPropertyName("type")]
    public GroupType GroupType { get; set; }

    [JsonPropertyName("state")]
    public State State { get; set; }

    [JsonPropertyName("permissions")]
    public long Permissions { get; set; }

    [JsonPropertyName("activeKeysetUuid")]
    public string ActiveKeysetUuid { get; set; } = "";

    [JsonPropertyName("pubKey")]
    public JsonWebKey PublicKey { get; set; } = new();

    [JsonPropertyName("attrVersion")]
    public int AttributesVersion { get; set; }
}