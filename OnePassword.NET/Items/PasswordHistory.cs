namespace OnePassword.Items;

public class PasswordHistory
{
    [JsonPropertyName("time")]
    public int Time { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = "";
}