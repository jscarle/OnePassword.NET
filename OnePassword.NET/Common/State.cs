using System.Runtime.Serialization;

namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State
{
    [EnumMember(Value = "A")]
    Active,

    [EnumMember(Value = "S")]
    Suspended
}