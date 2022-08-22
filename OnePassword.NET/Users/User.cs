using OnePassword.Common;

namespace OnePassword.Users;

public class User
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; init; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }

    [JsonPropertyName("lastAuthAt")]
    public DateTime LastAuthenticationAt { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; } = "";

    [JsonPropertyName("firstName")]
    public string FirstName { get; init; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("state")]
    public State State { get; init; }

    [JsonPropertyName("type")]
    public UserType UserType { get; init; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; init; } = "";

    [JsonPropertyName("language")]
    public string Language { get; init; } = "";

    [JsonPropertyName("accountKeyFormat")]
    public string AccountKeyFormat { get; init; } = "";

    [JsonPropertyName("accountKeyUuid")]
    public string AccountKeyUuid { get; init; } = "";

    [JsonPropertyName("attrVersion")]
    public int AttributesVersion { get; init; }

    [JsonPropertyName("keysetVersion")]
    public int KeysetVersion { get; init; }

    [JsonPropertyName("combinedPermissions")]
    public long CombinedPermissions { get; init; }
}