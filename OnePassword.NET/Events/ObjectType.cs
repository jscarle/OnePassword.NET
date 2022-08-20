using System.Runtime.Serialization;

namespace OnePassword.Events;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ObjectType
{
    [EnumMember(Value = "items")]
    Item,

    [EnumMember(Value = "file")]
    File,

    [EnumMember(Value = "device")]
    Device,

    [EnumMember(Value = "user")]
    User,

    [EnumMember(Value = "uva")]
    UserVaultAccess,

    [EnumMember(Value = "gva")]
    GroupVaultAccess,

    [EnumMember(Value = "vault")]
    Vault
}