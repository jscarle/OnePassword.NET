using OnePassword.Common;

namespace OnePassword.Documents;

/// <summary>
/// Represents a 1Password document.
/// </summary>
public sealed class Document : DocumentBase
{
    /// <summary>
    /// The document title.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("title")]
    public string Title { get; internal init; } = "";

    /// <summary>
    /// The tags associated with the document.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("tags")]
    public TrackedList<string> Tags { get; internal init; } = new();

    /// <summary>
    /// The document version.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("version")]
    public int Version { get; internal init; }

    /// <summary>
    /// The vault the document is stored in.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("vault")]
    public DocumentVault? Vault { get; internal init; }

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