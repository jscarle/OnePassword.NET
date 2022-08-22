namespace OnePassword.Items;

public class Section
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("title")]
    public string Title { get; init; } = "";

    [JsonPropertyName("fields")]
    public SectionFieldList Fields { get; init; } = new();
}