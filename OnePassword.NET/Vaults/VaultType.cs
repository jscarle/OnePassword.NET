using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultType>))]
public enum VaultType
{
    [EnumMember(Value = "Personal")]
    Personal,

    Unknown,

    [EnumMember(Value = "User Created")]
    User
}