using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the groups.</summary>
    /// <returns>The list of groups.</returns>
    public ImmutableList<Group> GetGroups();

    /// <summary>Gets a vault's groups.</summary>
    /// <param name="vault">The vault for which to retrieve groups.</param>
    /// <returns>The vault's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultGroup> GetGroups(IVault vault);

    /// <summary>Gets a user's groups.</summary>
    /// <param name="user">The user for which to retrieve groups.</param>
    /// <returns>The user's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<UserGroup> GetGroups(IUser user);

    /// <summary>Gets a vault's groups.</summary>
    /// <param name="vaultId">The ID of the vault for which to retrieve groups.</param>
    /// <returns>The vault's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultGroup> GetVaultGroups(string vaultId);

    /// <summary>Gets a user's groups.</summary>
    /// <param name="userId">The ID of the user for which to retrieve groups.</param>
    /// <returns>The user's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<UserGroup> GetUserGroups(string userId);

    /// <summary>Gets the group's details.</summary>
    /// <param name="group">The group to retrieve.</param>
    /// <returns>The group details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails GetGroup(IGroup group);

    /// <summary>Gets the group's details.</summary>
    /// <param name="groupId">The ID of the group to retrieve.</param>
    /// <returns>The group details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails GetGroup(string groupId);

    /// <summary>Create a group.</summary>
    /// <param name="name">The group name.</param>
    /// <param name="description">The group description.</param>
    /// <returns>The created group.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails CreateGroup(string name, string? description = null);

    /// <summary>Edits a group.</summary>
    /// <param name="group">The group to edit.</param>
    /// <param name="name">The group's new name.</param>
    /// <param name="description">The group's new description.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditGroup(IGroup group, string? name = null, string? description = null);

    /// <summary>Edits a group.</summary>
    /// <param name="groupId">The ID of the group to edit.</param>
    /// <param name="name">The group's new name.</param>
    /// <param name="description">The group's new description.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditGroup(string groupId, string? name = null, string? description = null);

    /// <summary>Deletes a group.</summary>
    /// <param name="group">The group to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteGroup(IGroup group);

    /// <summary>Deletes a group.</summary>
    /// <param name="groupId">The ID of the group to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteGroup(string groupId);

    /// <summary>Grants a user access to a group.</summary>
    /// <param name="group">The group to grant access from.</param>
    /// <param name="user">The user to grant access to.</param>
    /// <param name="userRole">The user role to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantAccess(IGroup group, IUser user, UserRole userRole = UserRole.Member);

    /// <summary>Grants a user access to a group.</summary>
    /// <param name="groupId">The ID of the group to grant access from.</param>
    /// <param name="userId">The ID of the user to grant access to.</param>
    /// <param name="userRole">The user role to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantAccess(string groupId, string userId, UserRole userRole = UserRole.Member);

    /// <summary>Revokes a user's access to a group.</summary>
    /// <param name="group">The group to revoke access from.</param>
    /// <param name="user">The user to revoke access to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeAccess(IGroup group, IUser user);

    /// <summary>Revokes a user's access to a group.</summary>
    /// <param name="groupId">The ID of the group to revoke access from.</param>
    /// <param name="userId">The ID of the user to revoke access to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeAccess(string groupId, string userId);
}