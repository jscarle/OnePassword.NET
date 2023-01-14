using OnePassword.Common;

namespace OnePassword.Vaults;

/// <summary>
/// Represents the type of 1Password vault.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<VaultType>))]
public enum VaultType
{
    /// <summary>
    /// Personal
    /// </summary>
    [EnumMember(Value = "Personal")]
    Personal,

    /// <summary>
    /// The vault type is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// User Created
    /// </summary>
    [EnumMember(Value = "User Created")]
    User
}