namespace OnePassword.Items;

public class SectionField
{
    [JsonPropertyName("n")]
    public string Name { get; set; } = "";

    [JsonPropertyName("t")]
    public string Title { get; set; } = "";

    [JsonPropertyName("k")]
    public string FieldType { get; set; } = "";

    [JsonPropertyName("v")]
    public object? Value { get; set; }

    [JsonPropertyName("a")]
    public SectionFieldAdditional Additional { get; set; } = new();

    [JsonPropertyName("inputTraits")]
    public InputTraits InputTraits { get; set; } = new();
}