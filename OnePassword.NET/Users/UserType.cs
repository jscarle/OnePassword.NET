using System.Runtime.Serialization;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    [EnumMember(Value = "R")]
    Regular,

    [EnumMember(Value = "G")]
    Guest
}