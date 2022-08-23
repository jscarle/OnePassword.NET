using OnePassword.Common;

namespace OnePassword.Vaults;

[JsonConverter(typeof(JsonStringEnumConverterEx<VaultType>))]
public enum VaultType
{
    [EnumMember(Value = "PERSONAL")]
    Personal,

    Unknown,

    [EnumMember(Value = "USER_CREATED")]
    UserDefined
}