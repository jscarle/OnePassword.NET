using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserRole>))]
public enum UserRole
{
    [EnumMember(Value = "member")]
    Member,

    [EnumMember(Value = "manager")]
    Manager
}