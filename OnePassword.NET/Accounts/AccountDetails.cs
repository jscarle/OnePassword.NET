using OnePassword.Common;

namespace OnePassword.Accounts;

public record AccountDetails
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("domain")]
    public string Domain { get; set; } = "";

    [JsonPropertyName("type")]
    public AccountType AccountType { get; set; }

    [JsonPropertyName("state")]
    public State State { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}