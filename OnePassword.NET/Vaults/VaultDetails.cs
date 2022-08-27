namespace OnePassword.Vaults;

public sealed class VaultDetails : VaultBase
{
    [JsonInclude]
    [JsonPropertyName("type")]
    public VaultType Type { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("items")]
    public int Items { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }
}