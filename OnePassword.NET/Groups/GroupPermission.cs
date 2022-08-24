using OnePassword.Common;

namespace OnePassword.Groups;

[JsonConverter(typeof(JsonStringEnumConverterEx<GroupPermission>))]
public enum GroupPermission
{
    [EnumMember(Value = "ADD_PERSON")]
    AddPerson,

    [EnumMember(Value = "CHANGE_PERSON_NAME")]
    ChangePersonName,

    [EnumMember(Value = "CHANGE_TEAM_ATTRIBUTES")]
    ChangeTeamAttributes,

    [EnumMember(Value = "CHANGE_TEAM_DOMAIN")]
    ChangeTeamDomain,

    [EnumMember(Value = "CHANGE_TEAM_SETTINGS")]
    ChangeTeamSettings,

    [EnumMember(Value = "CREATE_VAULTS")]
    CreateVaults,

    [EnumMember(Value = "DELETE_PERSON")]
    DeletePerson,

    [EnumMember(Value = "DELETE_TEAM")]
    DeleteTeam,

    [EnumMember(Value = "MANAGE_BILLING")]
    ManageBilling,

    [EnumMember(Value = "MANAGE_GROUPS")]
    ManageGroups,

    [EnumMember(Value = "MANAGE_TEMPLATES")]
    ManageTemplates,

    [EnumMember(Value = "MANAGE_VAULTS")]
    ManageVaults,

    [EnumMember(Value = "RECOVER_ACCOUNTS")]
    RecoverAccounts,

    [EnumMember(Value = "SUSPEND_PERSON")]
    SuspendPerson,

    [EnumMember(Value = "SUSPEND_TEAM")]
    SuspendTeam,

    [EnumMember(Value = "VIEW_ACTIVITIES_LOG")]
    ViewActivitiesLog,

    [EnumMember(Value = "VIEW_ADMINISTRATIVE_SIDEBAR")]
    ViewAdministrativeSidebar,

    [EnumMember(Value = "VIEW_BILLING")]
    ViewBilling,

    [EnumMember(Value = "VIEW_PEOPLE")]
    ViewPeople,

    [EnumMember(Value = "VIEW_TEAM_SETTINGS")]
    ViewTeamSettings,

    [EnumMember(Value = "VIEW_TEMPLATES")]
    ViewTemplates,

    [EnumMember(Value = "VIEW_VAULTS")]
    ViewVaults
}