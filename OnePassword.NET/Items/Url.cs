namespace OnePassword.Items;

public sealed record Url
{
    [JsonPropertyName("label")]
    public string Label { get; init; } = "";

    [JsonPropertyName("href")]
    public string Href { get; init; } = "";

    [JsonPropertyName("primary")]
    public bool Primary { get; init; }
}