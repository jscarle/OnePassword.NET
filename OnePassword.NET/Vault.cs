using Newtonsoft.Json;
using OnePassword.Vaults;

namespace OnePassword
{
    public class Vault
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public VaultType VaultType { get; set; }

        public VaultIcon Icon { get; set; } = VaultIcon.Default;
    }
}
