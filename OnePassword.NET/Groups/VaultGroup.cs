using OnePassword.Vaults;

namespace OnePassword.Groups;

/// <summary>
/// Represents a 1Password group associated with a vault.
/// </summary>
public sealed class VaultGroup : GroupBase
{
    /// <summary>
    /// The group's permissions for the vault.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<VaultPermission> Permissions { get; internal init; } = ImmutableList<VaultPermission>.Empty;
}