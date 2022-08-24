using OnePassword.Common;

namespace OnePassword.Users;

public sealed record User : IUser
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("email")]
    public string Email { get; init; } = "";

    [JsonPropertyName("type")]
    public UserType Type { get; init; } = UserType.Unknown;

    [JsonPropertyName("state")]
    public State State { get; init; } = State.Unknown;

    [JsonPropertyName("created_at")]
    public DateTimeOffset? Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; init; }

    [JsonPropertyName("last_auth_at")]
    public DateTimeOffset? LastAuthentication { get; init; }
}