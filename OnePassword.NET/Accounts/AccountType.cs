using OnePassword.Common;

namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverterEx<AccountType>))]
public enum AccountType
{
    [EnumMember(Value = "PERSONAL")]
    Personal,

    [EnumMember(Value = "BUSINESS")]
    Business
}