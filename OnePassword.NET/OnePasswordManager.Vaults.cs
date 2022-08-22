using System.Collections.Immutable;
using OnePassword.Groups;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Vault> GetVaults() => Op<ImmutableList<Vault>>("vault list");

    public ImmutableList<Vault> GetVaults(User user) => Op<ImmutableList<Vault>>($"vault list --user {user.Uuid}");

    public ImmutableList<Vault> GetVaults(Group group) => Op<ImmutableList<Vault>>($"vault list --group {group.Uuid}");

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

    private static string GetIconName(VaultIcon vaultIcon)
    {
        var field = vaultIcon.GetType().GetField(vaultIcon.ToString());
        if (field is null)
            throw new Exception("Could not find enum value.");

        var attributes = (EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
        if (attributes.Length == 0 || attributes[0].Value is null)
            throw new NotImplementedException("Icon value string has not been defined for this icon.");

        return attributes[0].Value;
    }
}