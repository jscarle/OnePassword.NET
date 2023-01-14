using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <summary>
    /// Gets the vaults.
    /// </summary>
    /// <returns>The list of vaults.</returns>
    public ImmutableList<Vault> GetVaults()
    {
        const string command = "vault list";
        return Op<ImmutableList<Vault>>(command);
    }

    /// <summary>
    /// Gets a vault.
    /// </summary>
    /// <param name="vault">The vault to retrieve.</param>
    /// <returns>The vault details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails GetVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault get {vault.Id}";
        return Op<VaultDetails>(command);
    }

    /// <summary>
    /// Creates a vault.
    /// </summary>
    /// <param name="name">The vault name.</param>
    /// <param name="description">The vault description.</param>
    /// <param name="icon">The vault icon.</param>
    /// <param name="allowAdminsToManage">When <see langword="true"/>, allows administrators to manage the vault.</param>
    /// <returns>The created vault.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Edits a vault.
    /// </summary>
    /// <param name="vault">The vault to edit.</param>
    /// <param name="name">The vault's new name.</param>
    /// <param name="description">The vault's new description.</param>
    /// <param name="icon">The vault's new icon.</param>
    /// <param name="travelMode">When <see langword="true"/>, enables travel mode on the vault. If enabled, <see langword="false"/> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
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

    /// <summary>
    /// Deletes a vault.
    /// </summary>
    /// <param name="vault">The vault to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault delete {vault.Id}";
        Op(command);
    }

    /// <summary>
    /// Grants a group permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="group">The group to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Grants a user permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="user">The user to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Revokes a group's permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <param name="group">The group to revoke permissions to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Revokes a user's permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <param name="user">The user to revoke permissions to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Gets a group's vaults.
    /// </summary>
    /// <param name="group">The group to retrieve vaults for.</param>
    /// <returns>The group's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"vault list --group {group.Id}";
        return Op<ImmutableList<Vault>>(command);
    }

    /// <summary>
    /// Gets a user's vaults.
    /// </summary>
    /// <param name="user">The user to retrieve vaults for.</param>
    /// <returns>The user's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"vault list --user {user.Id}";
        return Op<ImmutableList<Vault>>(command);
    }
}