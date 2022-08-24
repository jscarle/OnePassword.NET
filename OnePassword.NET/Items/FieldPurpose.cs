using OnePassword.Common;

namespace OnePassword.Items;

[JsonConverter(typeof(JsonStringEnumConverterEx<FieldPurpose>))]
public enum FieldPurpose
{
    [EnumMember(Value = "Notes")]
    Notes,

    [EnumMember(Value = "Password")]
    Password,

    Unknown,

    [EnumMember(Value = "Username")]
    Username
}