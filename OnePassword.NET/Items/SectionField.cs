namespace OnePassword.Items;

public class SectionField
{
    [JsonPropertyName("n")]
    public string Name { get; init; } = "";

    [JsonPropertyName("t")]
    public string Title { get; init; } = "";

    [JsonPropertyName("k")]
    public string FieldType { get; init; } = "";

    [JsonPropertyName("v")]
    public object? Value { get; init; }

    [JsonPropertyName("a")]
    public SectionFieldAdditional Additional { get; init; } = new();

    [JsonPropertyName("inputTraits")]
    public InputTraits InputTraits { get; init; } = new();
}