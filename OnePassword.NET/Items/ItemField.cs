namespace OnePassword.Items;

public class ItemField
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = "";

    [JsonPropertyName("designation")]
    public string Designation { get; init; } = "";

    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("type")]
    public string FieldType { get; init; } = "";

    [JsonPropertyName("value")]
    public object? Value { get; init; }
}