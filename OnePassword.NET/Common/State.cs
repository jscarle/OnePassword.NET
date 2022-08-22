namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverterEx<State>))]
public enum State
{
    [EnumMember(Value = "ACTIVE")]
    Active,

    [EnumMember(Value = "SUSPENDED")]
    Suspended
}