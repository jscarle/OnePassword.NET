using OnePassword.Common;

namespace OnePassword.Groups;

[JsonConverter(typeof(JsonStringEnumConverterEx<GroupType>))]
public enum GroupType
{
    [EnumMember(Value = "Administrators")]
    Administrators,

    [EnumMember(Value = "Owners")]
    Owners,

    [EnumMember(Value = "Recovery")]
    Recovery,

    [EnumMember(Value = "Team Members")]
    TeamMembers,

    Unknown,

    [EnumMember(Value = "User Defined")]
    User
}