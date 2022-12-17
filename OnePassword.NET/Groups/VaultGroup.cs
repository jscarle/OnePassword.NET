using OnePassword.Vaults;

namespace OnePassword.Groups;

public sealed class VaultGroup : GroupBase
{
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<VaultPermission> Permissions { get; internal init; } = ImmutableList<VaultPermission>.Empty;
}