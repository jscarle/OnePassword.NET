using OnePassword.Common;

namespace OnePassword.Users;

/// <summary>
/// Common base class that represents a 1Password user.
/// </summary>
public abstract class UserBase : ResultBase<IUser>, IUser
{
    /// <summary>
    /// The user email.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("email")]
    public string Email { get; internal init; } = "";

    /// <summary>
    /// The user type.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("type")]
    public UserType Type { get; internal init; } = UserType.Unknown;

    /// <summary>
    /// The state of the user.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("state")]
    public State State { get; internal init; } = State.Unknown;
}