using System.Collections.Immutable;
using OnePassword.Common;

namespace OnePassword.Groups;

public sealed record Group: IGroup
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("description")]
    public string Description { get; init; } = "";

    [JsonPropertyName("permissions")]
    public ImmutableList<Permission>? Permissions { get; init; } = null;

    [JsonPropertyName("type")]
    public GroupType Type { get; init; } = GroupType.Unknown;

    [JsonPropertyName("state")]
    public State State { get; init; } = State.Unknown;

    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; init; }
}