using OnePassword.Common;

namespace OnePassword.Items;

/// <summary>
/// Represents the purpose of a 1Password item field.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<FieldPurpose>))]
public enum FieldPurpose
{
    /// <summary>
    /// Notes
    /// </summary>
    [EnumMember(Value = "Notes")]
    Notes,

    /// <summary>
    /// Password
    /// </summary>
    [EnumMember(Value = "Password")]
    Password,

    /// <summary>
    /// The field purpose is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Username
    /// </summary>
    [EnumMember(Value = "Username")]
    Username
}