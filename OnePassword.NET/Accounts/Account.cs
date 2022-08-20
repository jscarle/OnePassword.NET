using OnePassword.Common;

namespace OnePassword.Accounts;

public class Account
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("type")]
    public AccountType AccountType { get; set; }

    [JsonPropertyName("state")]
    public State State { get; set; }

    [JsonPropertyName("avatar")]
    public string Avatar { get; set; } = "";

    [JsonPropertyName("domain")]
    public string Domain { get; set; } = "";

    [JsonPropertyName("baseAvatarURL")]
    public string BaseAvatarUrl { get; set; } = "";

    [JsonPropertyName("baseAttachmentURL")]
    public string BaseAttachmentUrl { get; set; } = "";

    [JsonPropertyName("attrVersion")]
    public int AttributesVersion { get; set; }
}