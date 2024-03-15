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
        return Op(JsonContext.Default.ImmutableListVault, command);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetVaults(IGroup group)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        return GetGroupVaults(group.Id);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetVaults(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        return GetUserVaults(user.Id);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetGroupVaults(string groupId)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));

        var command = $"vault list --group {groupId}";
        return Op(JsonContext.Default.ImmutableListVault, command);
    }

    /// <inheritdoc />
    public ImmutableList<Vault> GetUserVaults(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"vault list --user {userId}";
        return Op(JsonContext.Default.ImmutableListVault, command);
    }

    /// <inheritdoc />
    public VaultDetails GetVault(IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetVault(vault.Id);
    }

    /// <inheritdoc />
    public VaultDetails GetVault(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"vault get {vaultId}";
        return Op(JsonContext.Default.VaultDetails, command);
    }

    /// <inheritdoc />
    public VaultDetails CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null)
    {
        if (name is null || name.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

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
        return Op(JsonContext.Default.VaultDetails, command);
    }

    /// <inheritdoc />
    public void EditVault(IVault vault, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        if (name is null && description is null && icon is VaultIcon.Default or VaultIcon.Unknown && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        EditVault(vault.Id);
    }

    /// <inheritdoc />
    public void EditVault(string vaultId, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        if (name is null && description is null && icon is VaultIcon.Default or VaultIcon.Unknown && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"vault edit {vaultId}";
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
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        DeleteVault(vault.Id);
    }

    /// <inheritdoc />
    public void DeleteVault(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"vault delete {vaultId}";
        Op(command);
    }

    /// <inheritdoc />
    public void GrantPermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        GrantGroupPermissions(vault.Id, group.Id, permissions);
    }

    /// <inheritdoc />
    public void GrantPermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        GrantUserPermissions(vault.Id, user.Id, permissions);
    }

    /// <inheritdoc />
    public void GrantGroupPermissions(string vaultId, string groupId, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault group grant --vault {vaultId} --group {groupId} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void GrantUserPermissions(string vaultId, string userId, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user grant --vault {vaultId} --user {userId} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void RevokePermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        RevokeGroupPermissions(vault.Id, group.Id, permissions);
    }

    /// <inheritdoc />
    public void RevokePermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        RevokeUserPermissions(vault.Id, user.Id, permissions);
    }

    /// <inheritdoc />
    public void RevokeGroupPermissions(string vaultId, string groupId, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user revoke --vault {vaultId} --group {groupId} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void RevokeUserPermissions(string vaultId, string userId, IReadOnlyCollection<VaultPermission> permissions)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));
        if (permissions is null || permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var command = $"vault user revoke --vault {vaultId} --user {userId} --permissions \"{permissions.ToCommaSeparated()}\"";
        Op(command);
    }
}
