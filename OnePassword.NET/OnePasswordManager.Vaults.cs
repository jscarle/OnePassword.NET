using System.Collections.Immutable;
using System.Text;
using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Vault> GetVaults()
    {
        return Op<ImmutableList<Vault>>("vault list");
    }

    public ImmutableList<Vault> GetVaults(IUser user)
    {
        if (user.Id.Length == 0)
            throw new ArgumentException($"{nameof(user.Id)} cannot be empty.", nameof(user));

        return Op<ImmutableList<Vault>>($"vault list --user {user.Id}");
    }

    public ImmutableList<Vault> GetVaults(IGroup group)
    {
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));

        return Op<ImmutableList<Vault>>($"vault list --group {group.Id}");
    }

    public Vault GetVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return Op<Vault>($"vault get {vault.Id}");
    }

    public Vault CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null)
    {
        var trimmedName = name.Trim();
        if (trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"vault create \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        if (icon != VaultIcon.Default)
            command += $" --icon {icon.ToStringEnum()}";
        if (allowAdminsToManage.HasValue)
            command += $" --allow-admins-to-manage {(allowAdminsToManage.Value ? "true" : "false")}";
        return Op<Vault>(command);
    }

    public void EditVault(IVault vault, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var trimmedName = name?.Trim();
        if (trimmedName is not null && trimmedName.Length == 0)
            throw new ArgumentException($"{nameof(name)} cannot be empty.", nameof(name));

        var trimmedDescription = description?.Trim();

        var command = $"vault edit {vault.Id}";
        if (trimmedName is not null)
            command += $" --name \"{trimmedName}\"";
        if (trimmedDescription is not null)
            command += $" --description \"{trimmedDescription}\"";
        if (icon != VaultIcon.Default)
            command += $" --icon {icon.ToStringEnum()}";
        if (travelMode.HasValue)
            command += $" --travel-mode {(travelMode.Value ? "on" : "off")}";
        Op(command);
    }

    public void DeleteVault(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        Op($"vault delete {vault.Id}");
    }

    public void GrantPermissions(IVault vault, IGroup group, ICollection<Permission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var permissionValues = new StringBuilder();

        var type = typeof(Permission);
        foreach (var permission in permissions)
        {
            var enumMember = type.GetMember(permission.ToString())[0];
            var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();
            if (attr is null)
                throw new NotImplementedException("Permission string attribute has not been defined for this permission.");

            var value = attr.Value;
            if (value is null)
                throw new NotImplementedException("Permission string attribute value has not been defined for this icon.");

            permissionValues.Append(value);
        }

        var permissionsList = string.Join(",", permissionValues);
        Op($"vault group grant --vault {vault.Id} --group {group.Id} --permissions {permissionsList}");
    }

    public void RevokePermissions(IVault vault, IGroup group, ICollection<Permission> permissions)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (group.Id.Length == 0)
            throw new ArgumentException($"{nameof(group.Id)} cannot be empty.", nameof(group));
        if (permissions.Count == 0)
            throw new ArgumentException($"{nameof(permissions)} cannot be empty.", nameof(permissions));

        var permissionValues = new StringBuilder();

        var type = typeof(Permission);
        foreach (var permission in permissions)
        {
            var enumMember = type.GetMember(permission.ToString())[0];
            var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();
            if (attr is null)
                throw new NotImplementedException("Permission string attribute has not been defined for this permission.");

            var value = attr.Value;
            if (value is null)
                throw new NotImplementedException("Permission string attribute value has not been defined for this icon.");

            permissionValues.Append(value);
        }

        var permissionsList = string.Join(",", permissionValues);
        Op($"vault group revoke --vault {vault.Id} --group {group.Id} --permissions {permissionsList}");
    }
}