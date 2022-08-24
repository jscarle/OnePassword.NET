namespace OnePassword.Common;

[JsonConverter(typeof(JsonStringEnumConverterEx<Permission>))]
public enum Permission
{
    [EnumMember(Value = "ADD_PERSON")]
    AddPerson,

    [EnumMember(Value = "allow_editing")]
    AllowEditing,

    [EnumMember(Value = "allow_managing")]
    AllowManaging,

    [EnumMember(Value = "allow_viewing")]
    AllowViewing,

    [EnumMember(Value = "archive_items")]
    ArchiveItems,

    [EnumMember(Value = "CHANGE_PERSON_NAME")]
    ChangePersonName,

    [EnumMember(Value = "CHANGE_TEAM_ATTRIBUTES")]
    ChangeTeamAttributes,

    [EnumMember(Value = "CHANGE_TEAM_DOMAIN")]
    ChangeTeamDomain,

    [EnumMember(Value = "CHANGE_TEAM_SETTINGS")]
    ChangeTeamSettings,

    [EnumMember(Value = "copy_and_share_items")]
    CopyAndShareItems,

    [EnumMember(Value = "create_items")]
    CreateItems,

    [EnumMember(Value = "CREATE_VAULTS")]
    CreateVaults,

    [EnumMember(Value = "delete_items")]
    DeleteItems,

    [EnumMember(Value = "DELETE_PERSON")]
    DeletePerson,

    [EnumMember(Value = "DELETE_TEAM")]
    DeleteTeam,

    [EnumMember(Value = "edit_items")]
    EditItems,

    [EnumMember(Value = "export_items")]
    ExportItems,

    [EnumMember(Value = "import_items")]
    ImportItems,

    [EnumMember(Value = "MANAGE_BILLING")]
    ManageBilling,

    [EnumMember(Value = "MANAGE_GROUPS")]
    ManageGroups,

    [EnumMember(Value = "MANAGE_TEMPLATES")]
    ManageTemplates,

    [EnumMember(Value = "MANAGE_VAULT")]
    ManageVault,

    [EnumMember(Value = "MANAGE_VAULTS")]
    ManageVaults,

    [EnumMember(Value = "print_items")]
    PrintItems,

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

    [EnumMember(Value = "view_and_copy_passwords")]
    ViewAndCopyPasswords,

    [EnumMember(Value = "VIEW_BILLING")]
    ViewBilling,

    [EnumMember(Value = "view_item_history")]
    ViewItemHistory,

    [EnumMember(Value = "view_items")]
    ViewItems,

    [EnumMember(Value = "VIEW_PEOPLE")]
    ViewPeople,

    [EnumMember(Value = "VIEW_TEAM_SETTINGS")]
    ViewTeamSettings,

    [EnumMember(Value = "VIEW_TEMPLATES")]
    ViewTemplates,

    [EnumMember(Value = "VIEW_VAULTS")]
    ViewVaults
}