using System.Runtime.Serialization;

namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State
{
    [EnumMember(Value = "ACTIVE")]
    Active,

    [EnumMember(Value = "SUSPENDED")]
    Suspended
}