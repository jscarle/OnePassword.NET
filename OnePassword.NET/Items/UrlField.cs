namespace OnePassword.Items;

public class UrlField
{
    [JsonPropertyName("l")]
    public string Label { get; set; } = "";

    [JsonPropertyName("u")]
    public string Url { get; set; } = "";
}