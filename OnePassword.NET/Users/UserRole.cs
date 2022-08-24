using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserRole>))]
public enum UserRole
{
    [EnumMember(Value = "MANAGER")]
    Manager,

    [EnumMember(Value = "MEMBER")]
    Member,

    Unknown
}