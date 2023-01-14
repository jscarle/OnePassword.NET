using OnePassword.Common;

namespace OnePassword.Groups;

/// <summary>
/// Represents type of 1Password group.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<GroupType>))]
public enum GroupType
{
    /// <summary>
    /// Administrators
    /// </summary>
    [EnumMember(Value = "Administrators")]
    Administrators,

    /// <summary>
    /// Owners
    /// </summary>
    [EnumMember(Value = "Owners")]
    Owners,

    /// <summary>
    /// Recovery
    /// </summary>
    [EnumMember(Value = "Recovery")]
    Recovery,

    /// <summary>
    /// Team Members
    /// </summary>
    [EnumMember(Value = "Team Members")]
    TeamMembers,

    /// <summary>
    /// The group type is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// The group type is user defined.
    /// </summary>
    [EnumMember(Value = "User Defined")]
    User
}