namespace OnePassword.Items;

public class ItemOverview
{
    [JsonPropertyName("icons")]
    public IconField Icon { get; init; } = new();

    [JsonPropertyName("title")]
    public string Title { get; init; } = "";

    [JsonPropertyName("ainfo")]
    public string AdditionalInfo { get; init; } = "";

    [JsonPropertyName("pgrng")]
    public bool? PasswordGenerated { get; init; }

    [JsonPropertyName("pbe")]
    public double? PasswordEntropy { get; init; }

    [JsonPropertyName("ps")]
    public double? PasswordStrength { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; } = "";

    [JsonPropertyName("autosubmit")]
    public string AutoSubmit { get; init; } = "";

    [JsonPropertyName("URLs")]
    public UrlFieldList Urls { get; init; } = new();

    [JsonPropertyName("tags")]
    public List<string> Tags { get; init; } = new();
}