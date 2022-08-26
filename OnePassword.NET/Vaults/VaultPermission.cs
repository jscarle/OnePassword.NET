using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultPermission>))]
public enum VaultPermission
{
    [EnumMember(Value = "Allow Editing")]
    AllowEditing,

    [EnumMember(Value = "Allow Managing")]
    AllowManaging,

    [EnumMember(Value = "Allow Viewing")]
    AllowViewing,

    [EnumMember(Value = "Archive Items")]
    ArchiveItems,

    [EnumMember(Value = "Copy And Share Items")]
    CopyAndShareItems,

    [EnumMember(Value = "Create Items")]
    CreateItems,

    [EnumMember(Value = "Delete Items")]
    DeleteItems,

    [EnumMember(Value = "Edit Items")]
    EditItems,

    [EnumMember(Value = "Export Items")]
    ExportItems,

    [EnumMember(Value = "Import Items")]
    ImportItems,

    [EnumMember(Value = "Manage Vault")]
    ManageVault,

    [EnumMember(Value = "Print Items")]
    PrintItems,

    [EnumMember(Value = "View And Copy Passwords")]
    ViewAndCopyPasswords,

    [EnumMember(Value = "View Item History")]
    ViewItemHistory,

    [EnumMember(Value = "View Items")]
    ViewItems
}