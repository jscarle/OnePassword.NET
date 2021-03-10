using Newtonsoft.Json;

namespace OnePassword.Accounts
{
    [JsonConverter(typeof(AccountTypeConverter))]
    public enum AccountType
    {
        Personal,
        Business
    }
}
