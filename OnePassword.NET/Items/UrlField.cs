namespace OnePassword.Items;

public class UrlField
{
    [JsonPropertyName("l")]
    public string Label { get; init; } = "";

    [JsonPropertyName("u")]
    public string Url { get; init; } = "";
}