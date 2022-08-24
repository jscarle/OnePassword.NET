namespace OnePassword.Vaults;

public sealed record Vault : IVault
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("type")]
    public VaultType Type { get; init; } = VaultType.Unknown;

    [JsonPropertyName("items")]
    public int? Items { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; init; }
}