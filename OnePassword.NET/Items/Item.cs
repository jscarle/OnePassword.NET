using OnePassword.Vaults;

namespace OnePassword.Items;

/// <summary>
/// Represents a 1Password item.
/// </summary>
public sealed class Item : ItemBase, IItem
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = "";

    /// <summary>
    /// The item vault.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("vault")]
    public Vault? Vault { get; internal set; }

    /// <summary>
    /// The ID of the user that last edited the item.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("last_edited_by")]
    public string? LastEditedUserId { get; internal set; }

    /// <summary>
    /// The date and time when the item was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset? Created { get; internal set; }

    /// <summary>
    /// The date and time when the item was last updated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? Updated { get; internal set; }
}