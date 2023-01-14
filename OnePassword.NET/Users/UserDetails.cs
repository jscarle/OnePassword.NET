namespace OnePassword.Users;

/// <summary>
/// Represents a 1Password user with details.
/// </summary>
public sealed class UserDetails : UserBase
{
    private readonly DateTimeOffset? _lastAuthentication;

    /// <summary>
    /// The date and time when the user was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    /// <summary>
    /// The date and time when the user was last updated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }

    /// <summary>
    /// The date and time when the user was last authenticated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("last_auth_at")]
    public DateTimeOffset? LastAuthentication
    {
        get => _lastAuthentication;
        internal init => _lastAuthentication = value == DateTimeOffset.MinValue ? null : value;
    }
}