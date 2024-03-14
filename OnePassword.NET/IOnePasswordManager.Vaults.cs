using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the vaults.</summary>
    /// <returns>The list of vaults.</returns>
    public ImmutableList<Vault> GetVaults();

    /// <summary>Gets a group's vaults.</summary>
    /// <param name="group">The group to retrieve vaults for.</param>
    /// <returns>The group's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IGroup group);

    /// <summary>Gets a user's vaults.</summary>
    /// <param name="user">The user to retrieve vaults for.</param>
    /// <returns>The user's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IUser user);

    /// <summary>Gets a group's vaults.</summary>
    /// <param name="groupId">The ID of the group to retrieve vaults for.</param>
    /// <returns>The group's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetGroupVaults(string groupId);

    /// <summary>Gets a user's vaults.</summary>
    /// <param name="userId">The ID of the user to retrieve vaults for.</param>
    /// <returns>The user's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetUserVaults(string userId);

    /// <summary>Gets a vault.</summary>
    /// <param name="vault">The vault to retrieve.</param>
    /// <returns>The vault details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails GetVault(IVault vault);

    /// <summary>Gets a vault.</summary>
    /// <param name="vaultId">The ID of the vault to retrieve.</param>
    /// <returns>The vault details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails GetVault(string vaultId);

    /// <summary>Creates a vault.</summary>
    /// <param name="name">The vault name.</param>
    /// <param name="description">The vault description.</param>
    /// <param name="icon">The vault icon.</param>
    /// <param name="allowAdminsToManage">When <see langword="true" />, allows administrators to manage the vault.</param>
    /// <returns>The created vault.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null);

    /// <summary>Edits a vault.</summary>
    /// <param name="vault">The vault to edit.</param>
    /// <param name="name">The vault's new name.</param>
    /// <param name="description">The vault's new description.</param>
    /// <param name="icon">The vault's new icon.</param>
    /// <param name="travelMode">When <see langword="true" />, enables travel mode on the vault. If enabled, <see langword="false" /> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditVault(IVault vault, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null);

    /// <summary>Edits a vault.</summary>
    /// <param name="vaultId">The ID of the vault to edit.</param>
    /// <param name="name">The vault's new name.</param>
    /// <param name="description">The vault's new description.</param>
    /// <param name="icon">The vault's new icon.</param>
    /// <param name="travelMode">When <see langword="true" />, enables travel mode on the vault. If enabled, <see langword="false" /> disables it.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditVault(string vaultId, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null);

    /// <summary>Deletes a vault.</summary>
    /// <param name="vault">The vault to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteVault(IVault vault);

    /// <summary>Deletes a vault.</summary>
    /// <param name="vaultId">The ID of the vault to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteVault(string vaultId);

    /// <summary>Grants a group permissions to a vault.</summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="group">The group to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantPermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Grants a user permissions to a vault.</summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="user">The user to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantPermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Grants a group permissions to a vault.</summary>
    /// <param name="vaultId">The ID of the vault to grant permissions from.</param>
    /// <param name="groupId">The ID of the group to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantGroupPermissions(string vaultId, string groupId, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Grants a user permissions to a vault.</summary>
    /// <param name="vaultId">The ID of the vault to grant permissions from.</param>
    /// <param name="userId">The ID of the user to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantUserPermissions(string vaultId, string userId, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Revokes a group's permissions to a vault.</summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="group">The group to revoke permissions to.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokePermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Revokes a user's permissions to a vault.</summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="user">The user to revoke permissions to.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokePermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Revokes a group's permissions to a vault.</summary>
    /// <param name="vaultId">The ID of the vault to revoke permissions from.</param>
    /// <param name="groupId">The ID of the group to revoke permissions to.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeGroupPermissions(string vaultId, string groupId, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>Revokes a user's permissions to a vault.</summary>
    /// <param name="vaultId">The ID of the vault to revoke permissions from.</param>
    /// <param name="userId">The ID of the user to revoke permissions to.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeUserPermissions(string vaultId, string userId, IReadOnlyCollection<VaultPermission> permissions);
}