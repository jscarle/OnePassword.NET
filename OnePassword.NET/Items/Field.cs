namespace OnePassword.Items;

public sealed class Field
{
    [JsonInclude]
    [JsonPropertyName("section")]
    public Section? Section { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("label")]
    public string Label { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("type")]
    public FieldType Type { get; internal init; } = FieldType.Unknown;

    [JsonInclude]
    [JsonPropertyName("purpose")]
    public FieldPurpose? Purpose { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("value")]
    public string Value { get; set; } = "";

    [JsonInclude]
    [JsonPropertyName("password_details")]
    public PasswordDetails? PasswordDetails { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("reference")]
    public string? Reference { get; internal init; }
}