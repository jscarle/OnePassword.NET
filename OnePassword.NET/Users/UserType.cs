using OnePassword.Common;

namespace OnePassword.Users;

/// <summary>
/// Represents the type of 1Password user.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<UserType>))]
public enum UserType
{
    /// <summary>
    /// Guest
    /// </summary>
    [EnumMember(Value = "Guest")]
    Guest,

    /// <summary>
    /// Member
    /// </summary>
    [EnumMember(Value = "Member")]
    Member,

    /// <summary>
    /// The user type is unknown. 
    /// </summary>
    Unknown
}