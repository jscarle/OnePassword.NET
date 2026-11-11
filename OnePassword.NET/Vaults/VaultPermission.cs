using System.Diagnostics.CodeAnalysis;
using OnePassword.Common;

namespace OnePassword.Vaults;

/// <summary>
/// Represents a 1Password vault permission.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<VaultPermission>))]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public enum VaultPermission
{
    /// <summary>
    /// Allow Editing
    /// </summary>
    [EnumMember(Value = "allow_editing")]
    AllowEditing,

    /// <summary>
    /// Allow Managing
    /// </summary>
    [EnumMember(Value = "allow_managing")]
    AllowManaging,

    /// <summary>
    /// Allow Viewing
    /// </summary>
    [EnumMember(Value = "allow_viewing")]
    AllowViewing,

    /// <summary>
    /// Archive Items
    /// </summary>
    [EnumMember(Value = "archive_items")]
    ArchiveItems,

    /// <summary>
    /// Copy and Share Items
    /// </summary>
    [EnumMember(Value = "copy_and_share_items")]
    CopyAndShareItems,

    /// <summary>
    /// Create Items
    /// </summary>
    [EnumMember(Value = "create_items")]
    CreateItems,

    /// <summary>
    /// Delete Items
    /// </summary>
    [EnumMember(Value = "delete_items")]
    DeleteItems,

    /// <summary>
    /// Edit Items
    /// </summary>
    [EnumMember(Value = "edit_items")]
    EditItems,

    /// <summary>
    /// Export Items
    /// </summary>
    [EnumMember(Value = "export_items")]
    ExportItems,

    /// <summary>
    /// Import Items
    /// </summary>
    [EnumMember(Value = "import_items")]
    ImportItems,

    /// <summary>
    /// Manage Vault
    /// </summary>
    [EnumMember(Value = "manage_vault")]
    ManageVault,

    /// <summary>
    /// Print Items
    /// </summary>
    [EnumMember(Value = "print_items")]
    PrintItems,

    /// <summary>
    /// View and Copy Passwords
    /// </summary>
    [EnumMember(Value = "view_and_copy_passwords")]
    ViewAndCopyPasswords,

    /// <summary>
    /// View Item History
    /// </summary>
    [EnumMember(Value = "view_item_history")]
    ViewItemHistory,

    /// <summary>
    /// View Items
    /// </summary>
    [EnumMember(Value = "view_items")]
    ViewItems,

    /// <summary>
    /// The vault permission is unknown.
    /// </summary>
    Unknown
}
