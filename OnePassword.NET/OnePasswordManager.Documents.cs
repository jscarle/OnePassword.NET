using OnePassword.Common;
using OnePassword.Documents;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<DocumentDetails> GetDocuments(IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetDocuments(vault.Id);
    }

    /// <inheritdoc />
    public ImmutableList<DocumentDetails> GetDocuments(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"document list --vault {vaultId}";
        return Op(JsonContext.Default.ImmutableListDocumentDetails, command);
    }

    /// <inheritdoc />
    public ImmutableList<DocumentDetails> SearchForDocuments(IVault vault, bool? includeArchive = null)
    {
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return SearchForDocuments(vault?.Id, includeArchive);
    }

    /// <inheritdoc />
    public ImmutableList<DocumentDetails> SearchForDocuments(string? vaultId = null, bool? includeArchive = null)
    {
        if (vaultId is not null && vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = "document list";
        if (vaultId is not null)
            command += $" --vault {vaultId}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        return Op(JsonContext.Default.ImmutableListDocumentDetails, command);
    }

    /// <inheritdoc />
    public void GetDocument(IDocument document, IVault vault, string filePath, string? fileMode = null)
    {
        if (document is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        GetDocument(document.Id, vault.Id, filePath, fileMode);
    }

    /// <inheritdoc />
    public void GetDocument(string documentId, string vaultId, string filePath, string? fileMode = null)
    {
        if (documentId is null || documentId.Length == 0)
            throw new ArgumentException($"{nameof(documentId)} cannot be empty.", nameof(documentId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        var trimmedFileMode = fileMode?.Trim();

        // Not specifying --force will hang waiting for user input if the file exists.
        var command = $"document get {documentId} --out-file \"{trimmedFilePath}\" --force --vault {vaultId}";
        if (trimmedFileMode is not null)
            command += $" --file-mode {trimmedFileMode}";
        Op(command);
    }

    /// <inheritdoc />
    public void SearchForDocument(IDocument document, string filePath, IVault? vault = null, bool? includeArchive = null, string? fileMode = null)
    {
        if (document is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        SearchForDocument(document.Id, filePath, vault?.Id, includeArchive, fileMode);
    }

    /// <inheritdoc />
    public void SearchForDocument(string documentId, string filePath, string? vaultId = null, bool? includeArchive = null, string? fileMode = null)
    {
        if (documentId is null || documentId.Length == 0)
            throw new ArgumentException($"{nameof(documentId)} cannot be empty.", nameof(documentId));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));
        if (vaultId is not null && vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var trimmedFileMode = fileMode?.Trim();

        // Not specifying --force will hang waiting for user input if the file exists.
        var command = $"document get {documentId} --out-file \"{trimmedFilePath}\" --force";
        if (vaultId is not null)
            command += $" --vault {vaultId}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        if (trimmedFileMode is not null)
            command += $" --file-mode {trimmedFileMode}";
        Op(command);
    }

    /// <inheritdoc />
    public Document CreateDocument(IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        return CreateDocument(vault.Id, filePath, fileName, title, tags);
    }

    /// <inheritdoc />
    public Document CreateDocument(string vaultId, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        var trimmedFileName = fileName?.Trim();
        var trimmedTitle = title?.Trim();

        var command = $"document create \"{trimmedFilePath}\" --vault {vaultId}";
        if (trimmedFileName is not null)
            command += $" --file-name \"{trimmedFileName}\"";
        if (trimmedTitle is not null)
            command += $" --title \"{trimmedTitle}\"";
        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";
        return Op(JsonContext.Default.Document, command);
    }

    /// <inheritdoc />
    public void ReplaceDocument(IDocument document, IVault vault, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (document is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        ReplaceDocument(document.Id, vault.Id, filePath, fileName, title, tags);
    }

    /// <inheritdoc />
    public void ReplaceDocument(string documentId, string vaultId, string filePath, string? fileName = null, string? title = null, IReadOnlyCollection<string>? tags = null)
    {
        if (documentId is null || documentId.Length == 0)
            throw new ArgumentException($"{nameof(documentId)} cannot be empty.", nameof(documentId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));
        if (!File.Exists(trimmedFilePath))
            throw new ArgumentException($"File '{trimmedFilePath}' was not found or could not be accessed.", nameof(filePath));

        var trimmedFileName = fileName?.Trim();
        var trimmedTitle = title?.Trim();

        var command = $"document edit {documentId} \"{trimmedFilePath}\" --vault {vaultId}";
        if (trimmedFileName is not null)
            command += $" --file-name \"{trimmedFileName}\"";
        if (trimmedTitle is not null)
            command += $" --title \"{trimmedTitle}\"";
        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void ArchiveDocument(IDocument document, IVault vault)
    {
        if (document is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        DeleteDocument(document.Id, vault.Id);
    }

    /// <inheritdoc />
    public void ArchiveDocument(string documentId, string vaultId)
    {
        if (documentId is null || documentId.Length == 0)
            throw new ArgumentException($"{nameof(documentId)} cannot be empty.", nameof(documentId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"document delete {documentId} --vault {vaultId} --archive";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteDocument(IDocument document, IVault vault)
    {
        if (document is null || document.Id.Length == 0)
            throw new ArgumentException($"{nameof(document.Id)} cannot be empty.", nameof(document));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        DeleteDocument(document.Id, vault.Id);
    }

    /// <inheritdoc />
    public void DeleteDocument(string documentId, string vaultId)
    {
        if (documentId is null || documentId.Length == 0)
            throw new ArgumentException($"{nameof(documentId)} cannot be empty.", nameof(documentId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"document delete {documentId} --vault {vaultId}";
        Op(command);
    }
}
