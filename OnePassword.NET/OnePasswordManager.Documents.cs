using OnePassword.Common;
using OnePassword.Documents;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<Document> GetDocuments(
        string? vault = null,
        bool includeArchive = false)
    {
        var command = $"document list";

        if (!string.IsNullOrWhiteSpace(vault))
        {
            var trimmedVault = vault.Trim();
            command += $" --vault \"{trimmedVault}\"";
        }

        if (includeArchive != false)
        {
            command += $" --include-archived";
        }

        return Op<ImmutableList<Document>>(command);
    }

    /// <inheritdoc />
    public CreateDocument CreateDocument(
        string filePath,
        string? fileName = null,
        string? title = null,
        string? vault = null,
        IReadOnlyCollection<string>? tags = null)
    {
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));

        var command = $"document create  \"{trimmedFilePath}\"";

        if (!string.IsNullOrWhiteSpace(fileName))
        {
            var trimmedFileName = fileName.Trim();
            command += $" --file-name \"{trimmedFileName}\"";
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            var trimmedTitle = title.Trim();
            command += $" --title \"{trimmedTitle}\"";
        }

        if (!string.IsNullOrWhiteSpace(vault))
        {
            var trimmedVault = vault.Trim();
            command += $" --vault \"{trimmedVault}\"";
        }

        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";

        return Op<CreateDocument>(command);
    }
}