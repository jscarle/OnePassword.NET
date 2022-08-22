namespace OnePassword.Accounts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    [EnumMember(Value = "PERSONAL")]
    Personal,

    [EnumMember(Value = "BUSINESS")]
    Business
}