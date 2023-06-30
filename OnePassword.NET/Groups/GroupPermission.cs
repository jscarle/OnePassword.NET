using System.Diagnostics.CodeAnalysis;
using OnePassword.Common;

namespace OnePassword.Groups;

/// <summary>
/// Represents a 1Password group permission.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<GroupPermission>))]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public enum GroupPermission
{
    /// <summary>
    /// Add Person
    /// </summary>
    [EnumMember(Value = "Add Person")]
    AddPerson,

    /// <summary>
    /// Change Person Name
    /// </summary>
    [EnumMember(Value = "Change Person Name")]
    ChangePersonName,

    /// <summary>
    /// Change Team Attributes
    /// </summary>
    [EnumMember(Value = "Change Team Attributes")]
    ChangeTeamAttributes,

    /// <summary>
    /// Change Team Domain
    /// </summary>
    [EnumMember(Value = "Change Team Domain")]
    ChangeTeamDomain,

    /// <summary>
    /// Change Team Settings
    /// </summary>
    [EnumMember(Value = "Change Team Settings")]
    ChangeTeamSettings,

    /// <summary>
    /// Create Vaults
    /// </summary>
    [EnumMember(Value = "Create Vaults")]
    CreateVaults,

    /// <summary>
    /// Delete Person
    /// </summary>
    [EnumMember(Value = "Delete Person")]
    DeletePerson,

    /// <summary>
    /// Delete Team
    /// </summary>
    [EnumMember(Value = "Delete Team")]
    DeleteTeam,

    /// <summary>
    /// Manage Billing
    /// </summary>
    [EnumMember(Value = "Manage Billing")]
    ManageBilling,

    /// <summary>
    /// Manage Groups
    /// </summary>
    [EnumMember(Value = "Manage Groups")]
    ManageGroups,

    /// <summary>
    /// Manage Templates
    /// </summary>
    [EnumMember(Value = "Manage Templates")]
    ManageTemplates,

    /// <summary>
    /// Manage Vaults
    /// </summary>
    [EnumMember(Value = "Manage Vaults")]
    ManageVaults,

    /// <summary>
    /// Recover Accounts
    /// </summary>
    [EnumMember(Value = "Recover Accounts")]
    RecoverAccounts,

    /// <summary>
    /// Suspend Person
    /// </summary>
    [EnumMember(Value = "Suspend Person")]
    SuspendPerson,

    /// <summary>
    /// Suspend Team
    /// </summary>
    [EnumMember(Value = "Suspend Team")]
    SuspendTeam,

    /// <summary>
    /// View Activities Log
    /// </summary>
    [EnumMember(Value = "View Activities Log")]
    ViewActivitiesLog,

    /// <summary>
    /// View Administrative Sidebar
    /// </summary>
    [EnumMember(Value = "View AdministrativeSidebar")]
    ViewAdministrativeSidebar,

    /// <summary>
    /// View Billing
    /// </summary>
    [EnumMember(Value = "View Billing")]
    ViewBilling,

    /// <summary>
    /// View People
    /// </summary>
    [EnumMember(Value = "View People")]
    ViewPeople,

    /// <summary>
    /// View Team Settings
    /// </summary>
    [EnumMember(Value = "View Team Settings")]
    ViewTeamSettings,

    /// <summary>
    /// View Templates
    /// </summary>
    [EnumMember(Value = "View Templates")]
    ViewTemplates,

    /// <summary>
    /// View Vaults
    /// </summary>
    [EnumMember(Value = "View Vaults")]
    ViewVaults,

    /// <summary>
    /// The group permission is unknown.
    /// </summary>
    Unknown
}