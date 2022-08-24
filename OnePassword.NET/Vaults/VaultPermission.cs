using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultPermission>))]
public enum VaultPermission
{
    [EnumMember(Value = "allow_editing")]
    AllowEditing,

    [EnumMember(Value = "allow_managing")]
    AllowManaging,

    [EnumMember(Value = "allow_viewing")]
    AllowViewing,

    [EnumMember(Value = "archive_items")]
    ArchiveItems,

    [EnumMember(Value = "copy_and_share_items")]
    CopyAndShareItems,

    [EnumMember(Value = "create_items")]
    CreateItems,

    [EnumMember(Value = "delete_items")]
    DeleteItems,

    [EnumMember(Value = "edit_items")]
    EditItems,

    [EnumMember(Value = "export_items")]
    ExportItems,

    [EnumMember(Value = "import_items")]
    ImportItems,

    [EnumMember(Value = "manage_vault")]
    ManageVault,

    [EnumMember(Value = "print_items")]
    PrintItems,

    [EnumMember(Value = "view_and_copy_passwords")]
    ViewAndCopyPasswords,

    [EnumMember(Value = "view_item_history")]
    ViewItemHistory,

    [EnumMember(Value = "view_items")]
    ViewItems
}