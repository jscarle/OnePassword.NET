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
}