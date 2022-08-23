using OnePassword.Vaults;

namespace OnePassword.Items;

public sealed record Item : IItem
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = "";

    [JsonPropertyName("category")]
    public Category Category { get; init; } = Category.Unknown;

    [JsonPropertyName("sections")]
    public List<Section> Sections { get; init; } = new();

    [JsonPropertyName("fields")]
    public List<Field> Fields { get; init; } = new();

    [JsonPropertyName("urls")]
    public List<Url> Urls { get; init; } = new();

    [JsonPropertyName("tags")]
    public List<string> Tags { get; init; } = new();

    [JsonPropertyName("vault")]
    public Vault? Vault { get; init; }

    [JsonPropertyName("additional_information")]
    public string? AdditionalInformation { get; init; }

    [JsonPropertyName("version")]
    public int? Version { get; init; }

    [JsonPropertyName("last_edited_by")]
    public string? LastEditedUserId { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? Created { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; init; }
}