using OnePassword.Vaults;

namespace OnePassword.Users;

/// <summary>
/// Represents a 1Password user associated with a vault.
/// </summary>
public sealed class VaultUser : UserBase
{
    /// <summary>
    /// The user's permissions for the vault.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<VaultPermission> Permissions { get; internal set; } = ImmutableList<VaultPermission>.Empty;
}