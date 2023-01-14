using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <returns>The list of users.</returns>
    public ImmutableList<User> GetUsers()
    {
        const string command = "user list";
        return Op<ImmutableList<User>>(command);
    }

    /// <summary>
    /// Gets the user's details.
    /// </summary>
    /// <param name="user">The user to retrieve.</param>
    /// <returns>The user details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails GetUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));
        
        var command = $"user get {user.Id}";
        return Op<UserDetails>(command);
    }

    /// <summary>
    /// Provisions a user.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="emailAddress">The user email address.</param>
    /// <param name="language">The user language.</param>
    /// <returns>The provisioned user.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Confirms a user.
    /// </summary>
    /// <param name="user">The user to confirm.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ConfirmUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user confirm {user.Id}";
        Op(command);
    }

    /// <summary>
    /// Confirms all users.
    /// </summary>
    public void ConfirmAllUsers()
    {
        const string command = "user confirm --all";
        Op(command);
    }

    /// <summary>
    /// Edits a user.
    /// </summary>
    /// <param name="user">The user to edit.</param>
    /// <param name="name">The user's new name.</param>
    /// <param name="travelMode">When <see langword="true"/>, enables travel mode on the vault. If enabled, <see langword="false"/> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
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

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user delete {user.Id}";
        Op(command);
    }

    /// <summary>
    /// Suspends a user.
    /// </summary>
    /// <param name="user">The user to suspend.</param>
    /// <param name="deauthorizeDevicesDelay">The number of seconds to delay deauthorizing devices after the user has been suspended.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SuspendUser(IUser user, int? deauthorizeDevicesDelay = null)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user suspend {user.Id}";
        if (deauthorizeDevicesDelay is not null)
            command += $" --deauthorize-devices-after {deauthorizeDevicesDelay.Value}s";
        Op(command);
    }

    /// <summary>
    /// Reactivates a user.
    /// </summary>
    /// <param name="user">The user to reactivate.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReactivateUser(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        var command = $"user reactivate {user.Id}";
        Op(command);
    }

    /// <summary>
    /// Gets a vault's users.
    /// </summary>
    /// <param name="vault">The vault for which to retrieve users.</param>
    /// <returns>The vault's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultUser> GetUsers(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"vault user list {vault.Id}";
        return Op<ImmutableList<VaultUser>>(command);
    }

    /// <summary>
    /// Gets a group's users.
    /// </summary>
    /// <param name="group">The group for which to retrieve users.</param>
    /// <returns>The group's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<GroupUser> GetUsers(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        var command = $"group user list {group.Id}";
        return Op<ImmutableList<GroupUser>>(command);
    }
}