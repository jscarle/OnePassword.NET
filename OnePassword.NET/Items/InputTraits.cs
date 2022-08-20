namespace OnePassword.Items;

public class InputTraits
{
    [JsonPropertyName("autocapitalization")]
    public string AutoCapitalization { get; set; } = "";

    [JsonPropertyName("autocorrection")]
    public string AutoCorrection { get; set; } = "";

    [JsonPropertyName("keyboard")]
    public string Keyboard { get; set; } = "";
}