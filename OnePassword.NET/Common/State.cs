namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverterEx<>))]
public enum State
{
    [EnumMember(Value = "ACTIVE")]
    Active,

    [EnumMember(Value = "SUSPENDED")]
    Suspended
}