namespace OnePassword.Users;

public sealed class UserDetails : UserBase
{
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("last_auth_at")]
    public DateTimeOffset LastAuthentication { get; internal init; }
}