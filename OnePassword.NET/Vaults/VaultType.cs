using Newtonsoft.Json;

namespace OnePassword.Vaults
{
    [JsonConverter(typeof(VaultTypeConverter))]
    public enum VaultType
    {
        Personal,
        User
    }
}
