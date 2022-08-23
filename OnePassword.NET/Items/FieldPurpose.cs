using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<FieldPurpose>))]
public enum FieldPurpose
{
    [EnumMember(Value = "NOTES")]
    Notes,

    [EnumMember(Value = "PASSWORD")]
    Password,

    [EnumMember(Value = "USERNAME")]
    Username
}