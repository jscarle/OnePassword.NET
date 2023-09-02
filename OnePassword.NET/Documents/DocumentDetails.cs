using OnePassword.Vaults;

namespace OnePassword.Documents;

/// <summary>
/// Represents a 1Password document with details.
/// </summary>
public sealed class DocumentDetails : IDocument
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    /// <summary>
    /// The document title.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("title")]
    public string Title { get; internal init; } = "";

    /// <summary>
    /// The vault the document is stored in.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("vault")]
    public Vault? Vault { get; internal init; }

    /// <summary>
    /// The ID of the user that last edited the document.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("last_edited_by")]
    public string? LastEditedUserId { get; internal init; }

    /// <summary>
    /// The date and time when the document was created.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("created_at")]
    public DateTimeOffset Created { get; internal init; }

    /// <summary>
    /// The date and time when the document was updated.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("updated_at")]
    public DateTimeOffset Updated { get; internal init; }
}