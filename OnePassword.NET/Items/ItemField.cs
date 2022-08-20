namespace OnePassword.Items;

public class ItemField
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("designation")]
    public string Designation { get; set; } = "";

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("type")]
    public string FieldType { get; set; } = "";

    [JsonPropertyName("value")]
    public object? Value { get; set; }
}