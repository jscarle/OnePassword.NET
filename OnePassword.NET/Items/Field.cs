namespace OnePassword.Items;

public sealed record Field
{
    [JsonPropertyName("section")]
    public Section? Section { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("label")]
    public string Label { get; init; } = "";

    [JsonPropertyName("type")]
    public FieldType Type { get; init; } = FieldType.Unknown;

    [JsonPropertyName("purpose")]
    public FieldPurpose? Purpose { get; init; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = "";

    [JsonPropertyName("password_details")]
    public PasswordDetails? PasswordDetails { get; init; }

    [JsonPropertyName("reference")]
    public string? Reference { get; init; }
}