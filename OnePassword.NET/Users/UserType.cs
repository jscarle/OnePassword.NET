using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserType>))]
public enum UserType
{
    [EnumMember(Value = "GUEST")]
    Guest,

    [EnumMember(Value = "MEMBER")]
    Member,

    Unknown
}