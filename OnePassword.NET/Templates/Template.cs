namespace OnePassword.Templates;

public sealed record Template : ITemplate
{
    [JsonPropertyName("uuid")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";
}