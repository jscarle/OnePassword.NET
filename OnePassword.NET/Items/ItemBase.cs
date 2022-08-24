namespace OnePassword.Items;

public abstract class ItemBase
{
    [JsonInclude]
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonInclude]
    [JsonPropertyName("category")]
    public Category Category { get; internal init; } = Category.Unknown;

    [JsonInclude]
    [JsonPropertyName("sections")]
    public List<Section> Sections { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("fields")]
    public List<Field> Fields { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("urls")]
    public List<Url> Urls { get; internal init; } = new();

    [JsonInclude]
    [JsonPropertyName("tags")]
    public List<string> Tags { get; internal init; } = new();
}