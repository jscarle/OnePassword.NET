namespace OnePassword.Items;

public sealed record PasswordDetails
{
    [JsonPropertyName("strength")]
    public string Strength { get; init; } = "";
}