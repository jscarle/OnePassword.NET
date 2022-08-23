using OnePassword.Common;

namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverterEx<AccountType>))]
public enum AccountType
{
    [EnumMember(Value = "BUSINESS")]
    Business,

    [EnumMember(Value = "PERSONAL")]
    Personal,

    Unknown
}