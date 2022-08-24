using OnePassword.Common;

namespace OnePassword.Groups;

public abstract class GroupBase : ResultBase<IGroup>, IGroup
{
    [JsonInclude]
    [JsonPropertyName("description")]
    public string Description { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("state")]
    public State State { get; internal init; } = State.Unknown;

    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }
}