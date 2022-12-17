using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<User> GetUsers()
    {
        const string command = "user list";
        return Op<ImmutableList<User>>(command);
    }

    public UserDetails GetUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        
        var command = $"user get {user.Id}";
        return Op<UserDetails>(command);
    }

    public UserDetails ProvisionUser(string name, string emailAddress, Language language = Language.Default)
    {
        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedEmailAddress = emailAddress.Trim();
        if (trimmedEmailAddress.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var command = $"user provision --name \"{trimmedName}\" --email \"{trimmedEmailAddress}\"";
        if (language != Language.Default)
            command += $" --language \"{language.ToEnumString()}\"";
        return Op<UserDetails>(command);
    }

    public void ConfirmUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user confirm {user.Id}";
        Op(command);
    }

    public void ConfirmAllUsers()
    {
        const string command = "user confirm --all";
        Op(command);
    }

    public void EditUser(IUser user, string? name = null, bool? travelMode = null)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        if (name is null && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"user edit {user.Id}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (travelMode.HasValue)
            command += $" --travel-mode {(travelMode.Value ? "on" : "off")}";
        Op(command);
    }

    public void DeleteUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user delete {user.Id}";
        Op(command);
    }

    public void SuspendUser(IUser user, int? deauthorizeDevicesDelay = null)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user suspend {user.Id}";
        if (deauthorizeDevicesDelay is not null)
            command += $" --deauthorize-devices-after {deauthorizeDevicesDelay.Value}s";
        Op(command);
    }

    public void ReactivateUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user reactivate {user.Id}";
        Op(command);
    }

    public ImmutableList<VaultUser> GetUsers(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault user list {vault.Id}";
        return Op<ImmutableList<VaultUser>>(command);
    }

    public ImmutableList<GroupUser> GetUsers(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group user list {group.Id}";
        return Op<ImmutableList<GroupUser>>(command);
    }
}