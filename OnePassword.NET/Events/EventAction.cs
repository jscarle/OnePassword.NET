using System.Runtime.Serialization;

namespace OnePassword.Events;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EventAction
{
    [EnumMember(Value = "create")]
    Created,

    [EnumMember(Value = "update")]
    Updated,

    [EnumMember(Value = "updatea")]
    UpdatedAttribute,

    [EnumMember(Value = "patch")]
    UpdatedItem,

    [EnumMember(Value = "delete")]
    Deleted,

    [EnumMember(Value = "grant")]
    Granted,

    [EnumMember(Value = "revoke")]
    Revoked,

    [EnumMember(Value = "suspend")]
    Suspended,

    [EnumMember(Value = "reactive")]
    Reactivated
}