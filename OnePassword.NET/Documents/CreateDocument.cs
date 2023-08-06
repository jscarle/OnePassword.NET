namespace OnePassword.Documents
{
    internal class CreateDocument : DocumentBase
    {
        /// <summary>
        /// The document title.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("uuid")]
        public string UUId { get; internal init; } = "";

        /// <summary>
        /// The document title.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("vaultUuid")]
        public string VaultUUId { get; internal init; } = "";

        /// <summary>
        /// The date and time when the document was created.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("createdAt")]
        public DateTimeOffset Created { get; internal init; }

        /// <summary>
        /// The date and time when the document was updated.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("updatedAt")]
        public DateTimeOffset Updated { get; internal init; }
    }
}
