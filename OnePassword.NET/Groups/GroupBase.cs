using OnePassword.Common;

namespace OnePassword.Groups;

/// <summary>
/// Common base class that represents a 1Password group.
/// </summary>
public abstract class GroupBase : ResultBase<IGroup>, IGroup
{
    /// <summary>
    /// The group description.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("description")]
    public string Description { get; internal set; } = "";

    /// <summary>
    /// The state of the group.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("state")]
    public State State { get; internal set; } = State.Unknown;

    /// <summary>
    /// The date and time when the group was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal set; }
}