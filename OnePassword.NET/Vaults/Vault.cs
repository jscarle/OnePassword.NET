namespace OnePassword.Vaults;

public sealed record Vault : IVault
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    [JsonPropertyName("name")]
    public string Name { get; init; } = "";
}