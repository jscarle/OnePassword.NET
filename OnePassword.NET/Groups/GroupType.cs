using OnePassword.Common;

namespace OnePassword.Groups;

[JsonConverter(typeof(JsonStringEnumConverterEx<GroupType>))]
public enum GroupType
{
    [EnumMember(Value = "ADMINISTRATORS")]
    Administrators,

    [EnumMember(Value = "OWNERS")]
    Owners,

    [EnumMember(Value = "RECOVERY")]
    Recovery,

    [EnumMember(Value = "TEAM_MEMBERS")]
    TeamMembers,

    Unknown,

    [EnumMember(Value = "USER_DEFINED")]
    UserDefined
}