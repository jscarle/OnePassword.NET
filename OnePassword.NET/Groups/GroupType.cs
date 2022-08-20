using System.Runtime.Serialization;

namespace OnePassword.Groups;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GroupType
{
    [EnumMember(Value = "O")]
    Owner,

    [EnumMember(Value = "A")]
    Administrator,

    [EnumMember(Value = "R")]
    Recovery,

    [EnumMember(Value = "M")]
    TeamMember,

    [EnumMember(Value = "U")]
    User
}