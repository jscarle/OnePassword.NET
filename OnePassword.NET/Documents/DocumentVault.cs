namespace OnePassword.Documents
{
    /// <summary>
    /// Represents a 1Password document vault.
    /// </summary>
    public sealed class DocumentVault
    {
        /// <summary>
        /// The vault ID.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("id")]
        public string Id { get; internal init; } = "";

        /// <summary>
        /// The vault name.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("name")]
        public string Name { get; internal init; } = "";
    }
}
