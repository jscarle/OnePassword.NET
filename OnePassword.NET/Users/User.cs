using OnePassword.Common;

namespace OnePassword.Users;

public class User
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("lastAuthAt")]
    public DateTime LastAuthenticationAt { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = "";

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("state")]
    public State State { get; set; }

    [JsonPropertyName("type")]
    public UserType UserType { get; set; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = "";

    [JsonPropertyName("language")]
    public string Language { get; set; } = "";

    [JsonPropertyName("accountKeyFormat")]
    public string AccountKeyFormat { get; set; } = "";

    [JsonPropertyName("accountKeyUuid")]
    public string AccountKeyUuid { get; set; } = "";

    [JsonPropertyName("attrVersion")]
    public int AttributesVersion { get; set; }

    [JsonPropertyName("keysetVersion")]
    public int KeysetVersion { get; set; }

    [JsonPropertyName("combinedPermissions")]
    public long CombinedPermissions { get; set; }
}