using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <summary>
    /// Gets the groups.
    /// </summary>
    /// <returns>The list of groups.</returns>
    public ImmutableList<Group> GetGroups()
    {
        const string command = "group list";
        return Op<ImmutableList<Group>>(command);
    }

    /// <summary>
    /// Gets the group's details.
    /// </summary>
    /// <param name="group">The group to retrieve.</param>
    /// <returns>The group details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails GetGroup(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group get {group.Id}";
        return Op<GroupDetails>(command);
    }

    /// <summary>
    /// Create a group.
    /// </summary>
    /// <param name="name">The group name.</param>
    /// <param name="description">The group description.</param>
    /// <returns>The created group.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails CreateGroup(string name, string? description = null)
    {
        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"group create \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        return Op<GroupDetails>(command);
    }

    /// <summary>
    /// Edits a group.
    /// </summary>
    /// <param name="group">The group to edit.</param>
    /// <param name="name">The group's new name.</param>
    /// <param name="description">The group's new description.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditGroup(IGroup group, string? name = null, string? description = null)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        if (name is null && description is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"group edit {group.Id}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        Op(command);
    }

    /// <summary>
    /// Deletes a group.
    /// </summary>
    /// <param name="group">The group to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteGroup(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group delete {group.Id}";
        Op(command);
    }

    /// <summary>
    /// Grants a user access to a group.
    /// </summary>
    /// <param name="group">The group to grant access from.</param>
    /// <param name="user">The user to grant access to.</param>
    /// <param name="userRole">The user role to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantAccess(IGroup group, IUser user, UserRole userRole = UserRole.Member)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        if (userRole != UserRole.Member && userRole != UserRole.Manager)
            throw new ArgumentException($"{nameof(userRole)} must be {nameof(UserRole.Member)} or {nameof(UserRole.Manager)}.", nameof(userRole));

        var command = $"group user grant --group {group.Id} --user {user.Id} --role \"{userRole.ToEnumString()}\"";
        Op(command);
    }

    /// <summary>
    /// Revokes a user's access to a group.
    /// </summary>
    /// <param name="group">The group to revoke access from.</param>
    /// <param name="user">The user to revoke access to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeAccess(IGroup group, IUser user)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        
        var command = $"group user revoke --group {group.Id} --user {user.Id}";
        Op(command);
    }

    /// <summary>
    /// Gets a vault's groups.
    /// </summary>
    /// <param name="vault">The vault for which to retrieve groups.</param>
    /// <returns>The vault's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultGroup> GetGroups(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault group list {vault.Id}";
        return Op<ImmutableList<VaultGroup>>(command);
    }

    /// <summary>
    /// Gets a user's groups.
    /// </summary>
    /// <param name="user">The user for which to retrieve groups.</param>
    /// <returns>The user's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<UserGroup> GetGroups(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"group list --user {user.Id}";
        return Op<ImmutableList<UserGroup>>(command);
    }
}