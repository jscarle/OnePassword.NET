namespace OnePassword.Items;

public class Section
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("fields")]
    public SectionFieldList Fields { get; set; } = new();
}