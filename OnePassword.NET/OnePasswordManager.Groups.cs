using System.Collections.Immutable;
using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Group> GetGroups()
    {
        var command = "group list";
        return Op<ImmutableList<Group>>(command);
    }

    public Group GetGroup(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group get {group.Id}";
        return Op<Group>(command);
    }

    public Group CreateGroup(string name, string? description = null)
    {
        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"group create \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        return Op<Group>(command);
    }

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

    public void DeleteGroup(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group delete {group.Id}";
        Op(command);
    }

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

    public void RevokeAccess(IGroup group, IUser user)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        
        var command = $"group user revoke --group {group.Id} --user {user.Id}";
        Op(command);
    }

    public ImmutableList<User> GetUsers(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group user list {group.Id}";
        return Op<ImmutableList<User>>(command);
    }

    public ImmutableList<Vault> GetVaults(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"vault list --group {group.Id}";
        return Op<ImmutableList<Vault>>(command);
    }
}