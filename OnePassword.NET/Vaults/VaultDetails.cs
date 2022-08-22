namespace OnePassword.Vaults;

public sealed record VaultDetails : IVault
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("type")]
    public VaultType Type { get; init; }

    [JsonPropertyName("items")]
    public int Items { get; init; } = 0;

    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; init; }
}