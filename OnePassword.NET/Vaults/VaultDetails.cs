namespace OnePassword.Vaults;

public sealed record VaultDetails : IVault
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("content_version")]
    public int ContentVersion { get; init; } = 0;

    [JsonPropertyName("attribute_version")]
    public int AttributeVersion { get; init; } = 0;

    [JsonPropertyName("items")]
    public int Items { get; init; } = 0;

    [JsonPropertyName("type")]
    public VaultType Type { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; init; }
}