namespace OnePassword.Users;

public sealed class UserDetails : UserBase
{
    private readonly DateTimeOffset? _lastAuthentication;

    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("last_auth_at")]
    public DateTimeOffset? LastAuthentication
    {
        get => _lastAuthentication;
        internal init => _lastAuthentication = value == DateTimeOffset.MinValue ? null : value;
    }
}