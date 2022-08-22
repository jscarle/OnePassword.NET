namespace OnePassword.Items;

public class InputTraits
{
    [JsonPropertyName("autocapitalization")]
    public string AutoCapitalization { get; init; } = "";

    [JsonPropertyName("autocorrection")]
    public string AutoCorrection { get; init; } = "";

    [JsonPropertyName("keyboard")]
    public string Keyboard { get; init; } = "";
}