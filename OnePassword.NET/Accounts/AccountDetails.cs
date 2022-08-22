using OnePassword.Common;

namespace OnePassword.Accounts;

public sealed record AccountDetails
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("domain")]
    public string Domain { get; init; } = "";

    [JsonPropertyName("type")]
    public AccountType Type { get; init; }

    [JsonPropertyName("state")]
    public State State { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; init; }
}