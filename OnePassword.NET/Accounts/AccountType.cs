using System.Runtime.Serialization;

namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    [EnumMember(Value = "P")]
    Personal,

    [EnumMember(Value = "B")]
    Business
}