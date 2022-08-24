namespace OnePassword.Users;

public sealed class GroupUser : UserBase
{
    [JsonInclude]
    [JsonPropertyName("role")]
    public UserRole Role { get; internal init; } = UserRole.Unknown;
}