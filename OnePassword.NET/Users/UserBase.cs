using OnePassword.Common;

namespace OnePassword.Users;

public abstract class UserBase : ResultBase<IUser>, IUser
{
    [JsonInclude]
    [JsonPropertyName("email")]
    public string Email { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("type")]
    public UserType Type { get; internal init; } = UserType.Unknown;

    [JsonInclude]
    [JsonPropertyName("state")]
    public State State { get; internal init; } = State.Unknown;
}