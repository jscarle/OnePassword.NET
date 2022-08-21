using Newtonsoft.Json;

namespace OnePassword.Events
{
    [JsonConverter(typeof(ObjectTypeConverter))]
    public enum ObjectType
    {
        Item,
        File,
        Device,
        User,
        UserVaultAccess,
        GroupVaultAccess,
        Vault
    }
}
