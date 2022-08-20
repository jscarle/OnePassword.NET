namespace OnePassword.Items;

public class ItemOverview
{
    [JsonPropertyName("icons")]
    public IconField Icon { get; set; } = new();

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("ainfo")]
    public string AdditionalInfo { get; set; } = "";

    [JsonPropertyName("pgrng")]
    public bool? PasswordGenerated { get; set; }

    [JsonPropertyName("pbe")]
    public double? PasswordEntropy { get; set; }

    [JsonPropertyName("ps")]
    public double? PasswordStrength { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    [JsonPropertyName("autosubmit")]
    public string AutoSubmit { get; set; } = "";

    [JsonPropertyName("URLs")]
    public UrlFieldList Urls { get; set; } = new();

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();
}