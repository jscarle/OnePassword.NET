namespace OnePassword.Items;

public sealed class Url
{
    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("href")]
    public string Href { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("primary")]
    public bool Primary { get; internal init; }
}