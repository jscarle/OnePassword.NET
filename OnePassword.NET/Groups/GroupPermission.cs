using OnePassword.Common;

namespace OnePassword.Groups;

[JsonConverter(typeof(JsonStringEnumConverterEx<GroupPermission>))]
public enum GroupPermission
{
    [EnumMember(Value = "Add Person")]
    AddPerson,

    [EnumMember(Value = "Change Person Name")]
    ChangePersonName,

    [EnumMember(Value = "Change Team Attributes")]
    ChangeTeamAttributes,

    [EnumMember(Value = "Change Team Domain")]
    ChangeTeamDomain,

    [EnumMember(Value = "Change Team Settings")]
    ChangeTeamSettings,

    [EnumMember(Value = "Create Vaults")]
    CreateVaults,

    [EnumMember(Value = "Delete Person")]
    DeletePerson,

    [EnumMember(Value = "Delete Team")]
    DeleteTeam,

    [EnumMember(Value = "Manage Billing")]
    ManageBilling,

    [EnumMember(Value = "Manage Groups")]
    ManageGroups,

    [EnumMember(Value = "Manage Templates")]
    ManageTemplates,

    [EnumMember(Value = "Manage Vaults")]
    ManageVaults,

    [EnumMember(Value = "Recover Accounts")]
    RecoverAccounts,

    [EnumMember(Value = "Suspend Person")]
    SuspendPerson,

    [EnumMember(Value = "Suspend Team")]
    SuspendTeam,

    [EnumMember(Value = "View Activities Log")]
    ViewActivitiesLog,

    [EnumMember(Value = "View AdministrativeSidebar")]
    ViewAdministrativeSidebar,

    [EnumMember(Value = "View Billing")]
    ViewBilling,

    [EnumMember(Value = "View People")]
    ViewPeople,

    [EnumMember(Value = "View Team Settings")]
    ViewTeamSettings,

    [EnumMember(Value = "View Templates")]
    ViewTemplates,

    [EnumMember(Value = "View Vaults")]
    ViewVaults
}