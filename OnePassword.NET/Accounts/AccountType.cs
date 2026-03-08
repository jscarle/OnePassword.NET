using OnePassword.Common;

namespace OnePassword.Accounts;

/// <summary>
/// Represent a type of 1Password account.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<AccountType>))]
public enum AccountType
{
    /// <summary>
    /// 1Password Business Account
    /// </summary>
    [EnumMember(Value = "Business")]
    Business,

    /// <summary>
    /// 1Password Family Account
    /// </summary>
    [EnumMember(Value = "Family")]
    Family,

    /// <summary>
    /// 1Password Individual Account
    /// </summary>
    [EnumMember(Value = "Individual")]
    Individual,

    /// <summary>
    /// 1Password Personal Account
    /// </summary>
    [EnumMember(Value = "Personal")]
    Personal,

    /// <summary>
    /// 1Password Team Account
    /// </summary>
    [EnumMember(Value = "Team")]
    Team,

    /// <summary>
    /// The account type is unknown.
    /// </summary>
    Unknown
}
