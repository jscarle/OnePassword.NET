using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the users.</summary>
    /// <returns>The list of users.</returns>
    public ImmutableList<User> GetUsers();

    /// <summary>Gets a group's users.</summary>
    /// <param name="group">The group for which to retrieve users.</param>
    /// <returns>The group's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<GroupUser> GetUsers(IGroup group);

    /// <summary>Gets a vault's users.</summary>
    /// <param name="vault">The vault for which to retrieve users.</param>
    /// <returns>The vault's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultUser> GetUsers(IVault vault);

    /// <summary>Gets a group's users.</summary>
    /// <param name="groupId">The ID of the group for which to retrieve users.</param>
    /// <returns>The group's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<GroupUser> GetGroupUsers(string groupId);

    /// <summary>Gets a vault's users.</summary>
    /// <param name="vaultId">The ID of the vault for which to retrieve users.</param>
    /// <returns>The vault's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultUser> GetVaultUsers(string vaultId);

    /// <summary>Gets the user's details.</summary>
    /// <param name="user">The user to retrieve.</param>
    /// <returns>The user details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails GetUser(IUser user);

    /// <summary>Gets the user's details.</summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>The user details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails GetUser(string userId);

    /// <summary>Provisions a user.</summary>
    /// <param name="name">The user name.</param>
    /// <param name="emailAddress">The user email address.</param>
    /// <param name="language">The user language.</param>
    /// <returns>The provisioned user.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails ProvisionUser(string name, string emailAddress, Language language = Language.Default);

    /// <summary>Confirms a user.</summary>
    /// <param name="user">The user to confirm.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ConfirmUser(IUser user);

    /// <summary>Confirms a user.</summary>
    /// <param name="userId">The ID of the user to confirm.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ConfirmUser(string userId);

    /// <summary>Confirms all users.</summary>
    public void ConfirmAllUsers();

    /// <summary>Edits a user.</summary>
    /// <param name="user">The user to edit.</param>
    /// <param name="name">The user's new name.</param>
    /// <param name="travelMode">When <see langword="true" />, enables travel mode on the vault. If enabled, <see langword="false" /> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditUser(IUser user, string? name = null, bool? travelMode = null);

    /// <summary>Edits a user.</summary>
    /// <param name="userId">The ID of the user to edit.</param>
    /// <param name="name">The user's new name.</param>
    /// <param name="travelMode">When <see langword="true" />, enables travel mode on the vault. If enabled, <see langword="false" /> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditUser(string userId, string? name = null, bool? travelMode = null);

    /// <summary>Deletes a user.</summary>
    /// <param name="user">The user to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteUser(IUser user);

    /// <summary>Deletes a user.</summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteUser(string userId);

    /// <summary>Suspends a user.</summary>
    /// <param name="user">The user to suspend.</param>
    /// <param name="deauthorizeDevicesDelay">The number of seconds to delay deauthorizing devices after the user has been suspended.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SuspendUser(IUser user, int? deauthorizeDevicesDelay = null);

    /// <summary>Suspends a user.</summary>
    /// <param name="userId">The ID of the user to suspend.</param>
    /// <param name="deauthorizeDevicesDelay">The number of seconds to delay deauthorizing devices after the user has been suspended.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SuspendUser(string userId, int? deauthorizeDevicesDelay = null);

    /// <summary>Reactivates a user.</summary>
    /// <param name="user">The user to reactivate.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReactivateUser(IUser user);

    /// <summary>Reactivates a user.</summary>
    /// <param name="userId">The ID of the user to reactivate.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReactivateUser(string userId);
}