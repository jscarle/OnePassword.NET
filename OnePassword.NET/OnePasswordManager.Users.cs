using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<User> GetUsers()
    {
        const string command = "user list";
        return Op<ImmutableList<User>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<GroupUser> GetUsers(IGroup group)
    {
        if (group is null || group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        return GetGroupUsers(group.Id);
    }

    /// <inheritdoc />
    public ImmutableList<VaultUser> GetUsers(IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetVaultUsers(vault.Id);
    }

    /// <inheritdoc />
    public ImmutableList<GroupUser> GetGroupUsers(string groupId)
    {
        if (groupId is null || groupId.Length == 0)
            throw new ArgumentException($"{nameof(groupId)} cannot be empty.", nameof(groupId));

        var command = $"group user list {groupId}";
        return Op<ImmutableList<GroupUser>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<VaultUser> GetVaultUsers(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"vault user list {vaultId}";
        return Op<ImmutableList<VaultUser>>(command);
    }

    /// <inheritdoc />
    public UserDetails GetUser(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        return GetUser(user.Id);
    }

    /// <inheritdoc />
    public UserDetails GetUser(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"user get {userId}";
        return Op<UserDetails>(command);
    }

    /// <inheritdoc />
    public UserDetails ProvisionUser(string name, string emailAddress, Language language = Language.Default)
    {
        if (name is null || name.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));
        if (emailAddress is null || emailAddress.Length == 0)
            throw new ArgumentException($"{nameof(emailAddress)} cannot be empty.", nameof(emailAddress));

        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedEmailAddress = emailAddress.Trim();
        if (trimmedEmailAddress.Length == 0)
            throw new ArgumentException($"{nameof(emailAddress)} cannot be empty.", nameof(emailAddress));

        var command = $"user provision --name \"{trimmedName}\" --email \"{trimmedEmailAddress}\"";
        if (language != Language.Default)
            command += $" --language \"{language.ToEnumString()}\"";
        return Op<UserDetails>(command);
    }

    /// <inheritdoc />
    public void ConfirmUser(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        ConfirmUser(user.Id);
    }

    /// <inheritdoc />
    public void ConfirmUser(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"user confirm {userId}";
        Op(command);
    }

    /// <inheritdoc />
    public void ConfirmAllUsers()
    {
        const string command = "user confirm --all";
        Op(command);
    }

    /// <inheritdoc />
    public void EditUser(IUser user, string? name = null, bool? travelMode = null)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        if (name is null && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        EditUser(user.Id, name, travelMode);
    }

    /// <inheritdoc />
    public void EditUser(string userId, string? name = null, bool? travelMode = null)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        if (name is null && travelMode is null)
            throw new InvalidOperationException("Nothing to edit.");

        var command = $"user edit {userId}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (travelMode.HasValue)
            command += $" --travel-mode {(travelMode.Value ? "on" : "off")}";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteUser(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        DeleteUser(user.Id);
    }

    /// <inheritdoc />
    public void DeleteUser(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"user delete {userId}";
        Op(command);
    }

    /// <inheritdoc />
    public void SuspendUser(IUser user, int? deauthorizeDevicesDelay = null)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        SuspendUser(user.Id, deauthorizeDevicesDelay);
    }

    /// <inheritdoc />
    public void SuspendUser(string userId, int? deauthorizeDevicesDelay = null)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"user suspend {userId}";
        if (deauthorizeDevicesDelay is not null)
            command += $" --deauthorize-devices-after {deauthorizeDevicesDelay.Value}s";
        Op(command);
    }

    /// <inheritdoc />
    public void ReactivateUser(IUser user)
    {
        if (user is null || user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        ReactivateUser(user.Id);
    }

    /// <inheritdoc />
    public void ReactivateUser(string userId)
    {
        if (userId is null || userId.Length == 0)
            throw new ArgumentException($"{nameof(userId)} cannot be empty.", nameof(userId));

        var command = $"user reactivate {userId}";
        Op(command);
    }
}
