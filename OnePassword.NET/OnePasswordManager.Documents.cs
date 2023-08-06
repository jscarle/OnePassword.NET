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
            command += $" --include-archive";
        }

        return Op<ImmutableList<Document>>(command);
    }

    /// <inheritdoc />
    public string GetDocument(
        string nameOrId,
        string? outFile = null,
        string? fileMode = null,
        string? vault = null,
        bool includeArchive = false)
    {
        var trimmedNameOrId = nameOrId.Trim();
        if (trimmedNameOrId.Length == 0)
            throw new ArgumentException($"{nameof(nameOrId)} cannot be empty.", nameof(nameOrId));

        // TODO: Maybe support checking to see if a file would be overwritten on behalf of the force flag?
        var command = $"document get \"{trimmedNameOrId}\"";

        if (!string.IsNullOrWhiteSpace(outFile))
        {
            var trimmedOutFile = outFile.Trim();
            // Without the force flag, the CLI will prompt for confirmation.
            command += $" --force --out-file \"{trimmedOutFile}\"";
        }

        if (!string.IsNullOrWhiteSpace(fileMode))
        {
            var trimmedFileMode = fileMode.Trim();
            command += $" --file-mode \"{trimmedFileMode}\"";
        }

        if (!string.IsNullOrWhiteSpace(vault))
        {
            var trimmedVault = vault.Trim();
            command += $" --vault \"{trimmedVault}\"";
        }

        if (includeArchive != false)
        {
            command += $" --include-archive";
        }

        return Op(command);
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

    /// <inheritdoc />
    public void EditDocument(
        string nameOrId,
        string filePath,
        string? fileName = null,
        string? title = null,
        string? vault = null,
        IReadOnlyCollection<string>? tags = null)
    {
        var trimmedNameOrId = nameOrId.Trim();
        if (trimmedNameOrId.Length == 0)
            throw new ArgumentException($"{nameof(nameOrId)} cannot be empty.", nameof(nameOrId));

        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));

        var command = $"document edit \"{trimmedNameOrId}\" \"{trimmedFilePath}\"";

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

        Op(command);
    }

    /// <inheritdoc />
    public void DeleteDocument(
        string nameOrId,
        bool archive = false,
        string? vault = null)
    {
        var trimmedNameOrId = nameOrId.Trim();
        if (trimmedNameOrId.Length == 0)
            throw new ArgumentException($"{nameof(nameOrId)} cannot be empty.", nameof(nameOrId));

        var command = $"document delete \"{trimmedNameOrId}\"";

        if (archive != false)
        {
            command += $" --archive";
        }

        if (!string.IsNullOrWhiteSpace(vault))
        {
            var trimmedVault = vault.Trim();
            command += $" --vault \"{trimmedVault}\"";
        }

        Op(command);
    }
}