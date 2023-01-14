using OnePassword.Common;

namespace OnePassword.Users;

/// <summary>
/// Represents the role of a 1Password user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<UserRole>))]
public enum UserRole
{
    /// <summary>
    /// Manager
    /// </summary>
    [EnumMember(Value = "Manager")]
    Manager,

    /// <summary>
    /// Member
    /// </summary>
    [EnumMember(Value = "Member")]
    Member,

    /// <summary>
    /// The user role is unknown.
    /// </summary>
    Unknown
}