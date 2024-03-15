namespace OnePassword.Items;

/// <summary>
/// Represents details of a password.
/// </summary>
public sealed class PasswordDetails
{
    /// <summary>
    /// The password strength.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("strength")]
    public string Strength { get; internal set; } = "";
}