using OnePassword.Vaults;

namespace OnePassword.Users;

public sealed class VaultUser : UserBase
{
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<VaultPermission> Permissions { get; internal init; } = ImmutableList<VaultPermission>.Empty;
}