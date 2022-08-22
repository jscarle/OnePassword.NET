using OnePassword.Common;

namespace OnePassword.Users;

[JsonConverter(typeof(JsonStringEnumConverterEx<UserType>))]
public enum UserType
{
    [EnumMember(Value = "R")]
    Regular,

    [EnumMember(Value = "G")]
    Guest
}