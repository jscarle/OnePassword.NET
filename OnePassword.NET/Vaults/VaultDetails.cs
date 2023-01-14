namespace OnePassword.Vaults;

/// <summary>
/// Represents a 1Password vault with details.
/// </summary>
public sealed class VaultDetails : VaultBase
{
    /// <summary>
    /// The vault type.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("type")]
    public VaultType Type { get; internal init; }

    /// <summary>
    /// The vault items.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("items")]
    public int Items { get; internal init; }

    /// <summary>
    /// The date and time when the vault was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    /// <summary>
    /// The date and time when the vault was last updated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }
}