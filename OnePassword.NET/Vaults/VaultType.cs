using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultType>))]
public enum VaultType
{
    [EnumMember(Value = "PERSONAL")]
    Personal,

    [EnumMember(Value = "USER_CREATED")]
    User
}