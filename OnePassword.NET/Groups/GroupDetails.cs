namespace OnePassword.Groups;

/// <summary>
/// Represents a 1Password group with details.
/// </summary>
public sealed class GroupDetails : GroupBase
{
    /// <summary>
    /// The group permissions.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<GroupPermission> Permissions { get; internal init; } = ImmutableList<GroupPermission>.Empty;

    /// <summary>
    /// The group type.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("type")]
    public GroupType Type { get; internal init; } = GroupType.Unknown;

    /// <summary>
    /// The date and time when the group was last updated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }
}