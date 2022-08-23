using System.Collections.Immutable;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Group> GetGroups()
    {
        return Op<ImmutableList<Group>>("group list");
    }

    public ImmutableList<Group> GetGroups(IUser user)
    {
        if (user.Id.Length == 0)
            throw new Exception($"{nameof(user.Id)} cannot be empty.");

        return Op<ImmutableList<Group>>($"group list --user {user.Id}");
    }

    public ImmutableList<Group> GetGroups(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new Exception($"{nameof(vault.Id)} cannot be empty.");

        return Op<ImmutableList<Group>>($"vault group list {vault.Id}");
    }

    public Group GetGroup(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new Exception($"{nameof(group.Id)} cannot be empty.");

        return Op<Group>($"group get {group.Id}");
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
        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"group edit {group.Id}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        Op(command);
    }

    public void DeleteGroup(IGroup group)
    {
        Op($"group delete {group.Id}");
    }
}