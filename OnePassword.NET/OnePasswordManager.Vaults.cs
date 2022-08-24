using System.Collections.Immutable;
using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Vault> GetVaults()
    {
        var command = "vault list";
        return Op<ImmutableList<Vault>>(command);
    }

    public Vault GetVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault get {vault.Id}";
        return Op<Vault>(command);
    }

    public Vault CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null)
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
        return Op<Vault>(command);
    }

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

    public void DeleteVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault delete {vault.Id}";
        Op(command);
    }

    public void GrantPermissions(IVault vault, IGroup group, IReadOnlyCollection<Permission> permissions)
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

    public void GrantPermissions(IVault vault, IUser user, IReadOnlyCollection<Permission> permissions)
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

    public void RevokePermissions(IVault vault, IGroup group, IReadOnlyCollection<Permission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user revoke --vault {vault.Id} --group {group.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    public void RevokePermissions(IVault vault, IUser user, IReadOnlyCollection<Permission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user revoke --vault {vault.Id} --user {user.Id} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    public ImmutableList<Group> GetGroups(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault group list {vault.Id}";
        return Op<ImmutableList<Group>>(command);
    }

    public ImmutableList<User> GetUsers(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault user list {vault.Id}";
        return Op<ImmutableList<User>>(command);
    }
}