using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserRole>))]
public enum UserRole
{
    [EnumMember(Value = "Manager")]
    Manager,

    [EnumMember(Value = "Member")]
    Member,

    Unknown
}