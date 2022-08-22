namespace OnePassword.Accounts;

public record Account
{
    [JsonPropertyName("account_uuid")]
    public string Id { get; init; } = "";

    [JsonPropertyName("shorthand")]
    public string Shorthand { get; init; } = "";

    [JsonPropertyName("url")]
    public string Url { get; init; } = "";

    [JsonPropertyName("user_uuid")]
    public string UserId { get; init; } = "";

    [JsonPropertyName("email")]
    public string Email { get; init; } = "";
}