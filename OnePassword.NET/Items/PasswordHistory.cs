namespace OnePassword.Items;

public class PasswordHistory
{
    [JsonPropertyName("time")]
    public int Time { get; init; }

    [JsonPropertyName("value")]
    public string Value { get; init; } = "";
}