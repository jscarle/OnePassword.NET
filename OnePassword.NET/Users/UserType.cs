using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserType>))]
public enum UserType
{
    [EnumMember(Value = "Guest")]
    Guest,

    [EnumMember(Value = "Member")]
    Member,

    Unknown
}