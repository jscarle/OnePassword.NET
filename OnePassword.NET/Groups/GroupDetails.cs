namespace OnePassword.Groups;

public sealed class GroupDetails : GroupBase
{
    [JsonInclude]
    [JsonPropertyName("permissions")]
    public ImmutableList<GroupPermission> Permissions { get; internal init; } = ImmutableList<GroupPermission>.Empty;

    [JsonInclude]
    [JsonPropertyName("type")]
    public GroupType Type { get; internal init; } = GroupType.Unknown;

    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }
}