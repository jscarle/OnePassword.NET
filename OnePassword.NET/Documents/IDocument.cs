namespace OnePassword.Documents;

/// <summary>
/// Defines a 1Password document.
/// </summary>
public interface IDocument
{
    /// <summary>
    /// The document ID.
    /// </summary>
    string Id { get; }
}