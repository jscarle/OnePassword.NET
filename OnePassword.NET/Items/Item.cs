using OnePassword.Vaults;

namespace OnePassword.Items;

public sealed class Item : ItemBase, IItem
{
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("vault")]
    public Vault? Vault { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("last_edited_by")]
    public string? LastEditedUserId { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? Created { get; internal init; }

    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; internal init; }
}