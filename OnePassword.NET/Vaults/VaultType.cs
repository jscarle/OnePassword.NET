using System.Runtime.Serialization;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VaultType
{
    [EnumMember(Value = "P")]
    Personal,

    [EnumMember(Value = "U")]
    User
}