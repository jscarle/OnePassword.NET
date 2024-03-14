using OnePassword.Documents;
using OnePassword.Vaults;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets a vault's documents.</summary>
    /// <param name="vault">The vault that contains the documents to retrieve.</param>
    /// <returns>The vault's documents.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<DocumentDetails> GetDocuments(IVault vault);

    /// <summary>Gets a vault's documents.</summary>
    /// <param name="vaultId">The ID of the vault that contains the documents to retrieve.</param>
    /// <returns>The vault's documents.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<DocumentDetails> GetDocuments(string vaultId);

    /// <summary>Searches for an document.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="vault">The vault that contains the documents to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived documents in the search.</param>
    /// <returns>The documents that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<DocumentDetails> SearchForDocuments(IVault vault, bool? includeArchive = null);

    /// <summary>Searches for an document.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="vaultId">The ID of the vault that contains the documents to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived documents in the search.</param>
    /// <returns>The documents that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<DocumentDetails> SearchForDocuments(string? vaultId = null, bool? includeArchive = null);

    /// <summary>Gets a document.</summary>
    /// <param name="document">The document to retrieve.</param>
    /// <param name="vault">The vault that contains the document to retrieve.</param>
    /// <param name="filePath">The file path to save the document to.</param>
    /// <param name="fileMode">The file mode.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GetDocument(IDocument document, IVault vault, string filePath, string? fileMode = null);

    /// <summary>Gets a document.</summary>
    /// <param name="documentId">The ID of the document to retrieve.</param>
    /// <param name="vaultId">The ID of the vault that contains the document to retrieve.</param>
    /// <param name="filePath">The file path to save the document to.</param>
    /// <param name="fileMode">The file mode.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GetDocument(string documentId, string vaultId, string filePath, string? fileMode = null);

    /// <summary>Searches for a document.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="document">The document to search for.</param>
    /// <param name="filePath">The file path to save the document to.</param>
    /// <param name="vault">The vault that contains the document to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived documents in the search.</param>
    /// <param name="fileMode">The file mode.</param>
    /// <returns>The document that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SearchForDocument(IDocument document, string filePath, IVault? vault = null, bool? includeArchive = null, string? fileMode = null);

    /// <summary>Searches for a document.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="documentId">The ID of the document to search for.</param>
    /// <param name="filePath">The file path to save the document to.</param>
    /// <param name="vaultId">The ID of the vault that contains the document to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived documents in the search.</param>
    /// <param name="fileMode">The file mode.</param>
    /// <returns>The document that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SearchForDocument(string documentId, string filePath, string? vaultId = null, bool? includeArchive = null, string? fileMode = null);

    /// <summary>Creates a document.</summary>
    /// <param name="vault">The vault in which to create the document.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">The document's filename.</param>
    /// <param name="title">The document's title.</param>
    /// <param name="tags">The document's tags.</param>
    /// <returns>The created document.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Document CreateDocument(IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null);

    /// <summary>Creates a document.</summary>
    /// <param name="vaultId">The ID of the vault in which to create the document.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">The document's filename.</param>
    /// <param name="title">The document's title.</param>
    /// <param name="tags">The document's tags.</param>
    /// <returns>The created document.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Document CreateDocument(string vaultId, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null);

    /// <summary>Replaces a document.</summary>
    /// <param name="document">The document to replace.</param>
    /// <param name="vault">The vault that contains the document to replace.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">The document's filename.</param>
    /// <param name="title">The document's title.</param>
    /// <param name="tags">The document's tags.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReplaceDocument(IDocument document, IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null);

    /// <summary>Replaces a document.</summary>
    /// <param name="documentId">The ID of the document to replace.</param>
    /// <param name="vaultId">The ID of the vault that contains the document to replace.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">The document's filename.</param>
    /// <param name="title">The document's title.</param>
    /// <param name="tags">The document's tags.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReplaceDocument(string documentId, string vaultId, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null);

    /// <summary>Archives a document.</summary>
    /// <param name="document">The document to archive.</param>
    /// <param name="vault">The vault that contains the document to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveDocument(IDocument document, IVault vault);

    /// <summary>Archives a document.</summary>
    /// <param name="documentId">The ID of the document to archive.</param>
    /// <param name="vaultId">The ID of the vault that contains the document to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveDocument(string documentId, string vaultId);

    /// <summary>Deletes a document.</summary>
    /// <param name="document">The document to delete.</param>
    /// <param name="vault">The vault that contains the document to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteDocument(IDocument document, IVault vault);

    /// <summary>Deletes a document.</summary>
    /// <param name="documentId">The ID of the document to delete.</param>
    /// <param name="vaultId">The ID of the vault that contains the document to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteDocument(string documentId, string vaultId);
}