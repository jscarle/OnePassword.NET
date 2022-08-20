namespace OnePassword.Vaults;

public class Vault
{
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("desc")]
    public string Description { get; set; } = "";

    [JsonPropertyName("type")]
    public VaultType VaultType { get; set; }

    public VaultIcon Icon { get; set; } = VaultIcon.Default;
}