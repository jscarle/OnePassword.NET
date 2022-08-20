using System.Runtime.Serialization;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    [EnumMember(Value = "member")]
    Member,

    [EnumMember(Value = "manager")]
    Manager
}