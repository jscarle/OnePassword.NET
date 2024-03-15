using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<Group> GetGroups()
    {
        const string command = "group list";
        return Op(JsonContext.Default.ImmutableListGroup, command);
    }

    /// <inheritdoc />
    public ImmutableList<VaultGroup> GetGroups(IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetVaultGroups(vault.Id);
    }

    /// <inheritdoc />
    public ImmutableList<UserGroup> GetGroups(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        return GetUserGroups(user.Id);
    }

    /// <inheritdoc />
    public ImmutableList<VaultGroup> GetVaultGroups(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"vault group list {vaultId}";
        return Op(JsonContext.Default.ImmutableListVaultGroup, command);
    }

    /// <inheritdoc />
    public ImmutableList<UserGroup> GetUserGroups(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"group list --user {userId}";
        return Op(JsonContext.Default.ImmutableListUserGroup, command);
    }

    /// <inheritdoc />
    public GroupDetails GetGroup(IGroup group)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        return GetGroup(group.Id);
    }

    /// <inheritdoc />
    public GroupDetails GetGroup(string groupId)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));

        var command = $"group get {groupId}";
        return Op(JsonContext.Default.GroupDetails, command);
    }

    /// <inheritdoc />
    public GroupDetails CreateGroup(string name, string? description = null)
    {
        if (name is null || name.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"group create \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        return Op(JsonContext.Default.GroupDetails, command);
    }

    /// <inheritdoc />
    public void EditGroup(IGroup group, string? name = null, string? description = null)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        if (name is null && description is null)
            throw new InvalidOperationException("Nothing to edit.");

        EditGroup(group.Id, name, description);
    }

    /// <inheritdoc />
    public void EditGroup(string groupId, string? name = null, string? description = null)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        if (name is null && description is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"group edit {groupId}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteGroup(IGroup group)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        DeleteGroup(group.Id);
    }

    /// <inheritdoc />
    public void DeleteGroup(string groupId)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));

        var command = $"group delete {groupId}";
        Op(command);
    }

    /// <inheritdoc />
    public void GrantAccess(IGroup group, IUser user, UserRole userRole = UserRole.Member)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (userRole != UserRole.Member && userRole != UserRole.Manager)
            throw new ArgumentException($"{nameof(userRole)} must be {nameof(UserRole.Member)} or {nameof(UserRole.Manager)}.", nameof(userRole));

        GrantAccess(group.Id, user.Id, userRole);
    }

    /// <inheritdoc />
    public void GrantAccess(string groupId, string userId, UserRole userRole = UserRole.Member)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));
        if (userRole != UserRole.Member && userRole != UserRole.Manager)
            throw new ArgumentException($"{nameof(userRole)} must be {nameof(UserRole.Member)} or {nameof(UserRole.Manager)}.", nameof(userRole));

        var command = $"group user grant --group {groupId} --user {userId} --role \"{userRole.ToEnumString()}\"";
        Op(command);
    }

    /// <inheritdoc />
    public void RevokeAccess(IGroup group, IUser user)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        RevokeAccess(group.Id, user.Id);
    }

    /// <inheritdoc />
    public void RevokeAccess(string groupId, string userId)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"group user revoke --group {groupId} --user {userId}";
        Op(command);
    }
}
