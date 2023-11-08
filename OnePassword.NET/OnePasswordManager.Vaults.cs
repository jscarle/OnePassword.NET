using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<Vault> GetVaults()
    {
        const string command = "vault list";
        return Op<ImmutableList<Vault>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetVaults(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"vault list --group {group.Id}";
        return Op<ImmutableList<Vault>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetVaults(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"vault list --user {user.Id}";
        return Op<ImmutableList<Vault>>(command);
    }

    /// <inheritdoc />
    public VaultDetails GetVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault get {vault.Id}";
        return Op<VaultDetails>(command);
    }

    /// <inheritdoc />
    public VaultDetails CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null)
    {
        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"vault create \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        if (icon != VaultIcon.Default && icon != VaultIcon.Unknown)
            command += $" --icon \"{icon.ToEnumString()}\"";
        if (allowAdminsToManage.HasValue)
            command += $" --allow-admins-to-manage {(allowAdminsToManage.Value ? "true" : "false")}";
        return Op<VaultDetails>(command);
    }

    /// <inheritdoc />
    public void EditVault(IVault vault, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        if (name is null && description is null && icon is VaultIcon.Default or VaultIcon.Unknown && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"vault edit {vault.Id}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        if (icon != VaultIcon.Default && icon != VaultIcon.Unknown)
            command += $" --icon \"{icon.ToEnumString()}\"";
        if (travelMode.HasValue)
            command += $" --travel-mode {(travelMode.Value ? "on" : "off")}";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault delete {vault.Id}";
        Op(command);
    }

    /// <inheritdoc />
    public void GrantPermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault group grant --vault {vault.Id} --group {group.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void GrantPermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user grant --vault {vault.Id} --user {user.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void RevokePermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"vault user revoke --vault {vault.Id} --group {group.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void RevokePermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"vault user revoke --vault {vault.Id} --user {user.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }
}