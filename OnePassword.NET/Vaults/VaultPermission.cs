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
    [EnumMember(Value = "Allow Editing")]
    AllowEditing,

    /// <summary>
    /// Allow Managing
    /// </summary>
    [EnumMember(Value = "Allow Managing")]
    AllowManaging,

    /// <summary>
    /// Allow Viewing
    /// </summary>
    [EnumMember(Value = "Allow Viewing")]
    AllowViewing,

    /// <summary>
    /// Archive Items
    /// </summary>
    [EnumMember(Value = "Archive Items")]
    ArchiveItems,

    /// <summary>
    /// Copy and Share Items
    /// </summary>
    [EnumMember(Value = "Copy And Share Items")]
    CopyAndShareItems,

    /// <summary>
    /// Create Items
    /// </summary>
    [EnumMember(Value = "Create Items")]
    CreateItems,

    /// <summary>
    /// Delete Items
    /// </summary>
    [EnumMember(Value = "Delete Items")]
    DeleteItems,

    /// <summary>
    /// Edit Items
    /// </summary>
    [EnumMember(Value = "Edit Items")]
    EditItems,

    /// <summary>
    /// Export Items
    /// </summary>
    [EnumMember(Value = "Export Items")]
    ExportItems,

    /// <summary>
    /// Import Items
    /// </summary>
    [EnumMember(Value = "Import Items")]
    ImportItems,

    /// <summary>
    /// Manage Vault
    /// </summary>
    [EnumMember(Value = "Manage Vault")]
    ManageVault,

    /// <summary>
    /// Print Items
    /// </summary>
    [EnumMember(Value = "Print Items")]
    PrintItems,

    /// <summary>
    /// View and Copy Passwords
    /// </summary>
    [EnumMember(Value = "View And Copy Passwords")]
    ViewAndCopyPasswords,

    /// <summary>
    /// View Item History
    /// </summary>
    [EnumMember(Value = "View Item History")]
    ViewItemHistory,

    /// <summary>
    /// View Items
    /// </summary>
    [EnumMember(Value = "View Items")]
    ViewItems,

    /// <summary>
    /// The vault permission is unknown.
    /// </summary>
    Unknown
}