using Newtonsoft.Json;

namespace OnePassword.Users
{
    [JsonConverter(typeof(UserTypeConverter))]
    public enum UserType
    {
        Regular,
        Guest
    }
}
