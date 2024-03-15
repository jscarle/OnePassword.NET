namespace OnePassword.Documents;

/// <summary>
/// Represents a 1Password document.
/// </summary>
public sealed class Document : IDocument
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("uuid")]
    public string Id { get; internal set; } = "";
}