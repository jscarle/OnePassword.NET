using OnePassword.Common;

namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverterEx<AccountType>))]
public enum AccountType
{
    [EnumMember(Value = "Business")]
    Business,

    [EnumMember(Value = "Family")]
    Family,

    [EnumMember(Value = "Personal")]
    Personal,

    [EnumMember(Value = "Teams")]
    Teams,

    Unknown
}
