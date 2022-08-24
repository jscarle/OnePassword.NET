namespace OnePassword.Items;

public sealed class PasswordDetails
{
    [JsonInclude]
    [JsonPropertyName("strength")]
    public string Strength { get; internal init; } = "";
}