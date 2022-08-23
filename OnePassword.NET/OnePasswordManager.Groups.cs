using System.Collections.Immutable;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Group> GetGroups() => Op<ImmutableList<Group>>("group list");

    public ImmutableList<Group> GetGroups(User user) => Op<ImmutableList<Group>>($"group list --user {user.Uuid}");

    public ImmutableList<Group> GetGroups(IVault vault) => Op<ImmutableList<Group>>($"vault group list {vault.Id}");

    public Group GetGroup(IGroup group) => Op<Group>($"group get {group.Id}");

    public Group CreateGroup(string name, string description = "")
    {
        var command = $"group create \"{name}\"";
        if (description.Length > 0)
            command += $" --description \"{description}\"";
        return Op<Group>(command);
    }

    public void EditGroup(IGroup group, string name = "", string description = "")
    {
        var command = $"group edit {group.Id}";
        if (!string.IsNullOrEmpty(name))
            command += $" --name \"{name}\"";
        if (!string.IsNullOrEmpty(description))
            command += $" --description \"{description}\"";
        Op(command);
    }

    public void DeleteGroup(IGroup group) => Op($"group delete {group.Id}");
}