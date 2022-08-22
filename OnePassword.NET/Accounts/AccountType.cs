using OnePassword.Common;

namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverterEx<>))]
public enum AccountType
{
    [EnumMember(Value = "PERSONAL")]
    Personal,

    [EnumMember(Value = "BUSINESS")]
    Business
}