using System;
using System.Collections.Immutable;
using System.Text;
using OnePassword.Common;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Vault> GetVaults() => Op<ImmutableList<Vault>>("vault list");

    public ImmutableList<Vault> GetVaults(User user) => Op<ImmutableList<Vault>>($"vault list --user {user.Uuid}");

    public ImmutableList<Vault> GetVaults(IGroup group) => Op<ImmutableList<Vault>>($"vault list --group {group.Id}");

    public VaultDetails GetVault(IVault vault) => Op<VaultDetails>($"vault get {vault.Id}");

    public VaultDetails CreateVault(string name, string description = "", VaultIcon icon = VaultIcon.Default, bool allowAdminsToManage = false)
    {
        var command = $"vault create \"{name}\"";
        if (!string.IsNullOrEmpty(description))
            command += $" --description \"{description}\"";
        if (icon != VaultIcon.Default)
            command += $" --icon \"{GetIconName(icon)}\"";
        if (!allowAdminsToManage)
            command += " --allow-admins-to-manage \"false\"";
        return Op<VaultDetails>(command);
    }

    public void EditVault(IVault vault, string name = "", string description = "", VaultIcon icon = VaultIcon.Default, bool? travelMode = null)
    {
        var command = $"vault edit {vault.Id}";
        if (!string.IsNullOrEmpty(name))
            command += $" --name \"{name}\"";
        if (!string.IsNullOrEmpty(description))
            command += $" --description \"{description}\"";
        if (icon != VaultIcon.Default)
            command += $" --icon {GetIconName(icon)}";
        if (travelMode.HasValue)
            command += $" --travel-mode {(travelMode.Value ? "on" : "off")}";
        Op(command);
    }

    public void DeleteVault(IVault vault) => Op($"vault delete {vault.Id}");

    public ImmutableList<Group> GetVaultGroups(IVault vault) => Op<ImmutableList<Group>>($"vault group list {vault.Id}");

    public void GrantVaultPermissions(IVault vault, IGroup group, IEnumerable<Permission> permissions)
    {
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

    public void RevokeVaultPermissions(IVault vault, IGroup group, IEnumerable<Permission> permissions)
    {
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

    private static string GetIconName(VaultIcon vaultIcon)
    {
        var field = vaultIcon.GetType().GetField(vaultIcon.ToString());
        if (field is null)
            throw new Exception("Could not find enum value.");

        var attributes = (EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
        if (attributes.Length == 0)
            throw new NotImplementedException("Icon string attribute has not been defined for this icon.");

        var iconName = attributes[0].Value;
        if (iconName is null)
            throw new NotImplementedException("Icon string attribute value has not been defined for this icon.");

        return iconName;
    }
}