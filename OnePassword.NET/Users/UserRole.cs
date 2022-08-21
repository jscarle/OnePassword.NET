using Newtonsoft.Json;

namespace OnePassword.Users
{
    [JsonConverter(typeof(UserRoleConverter))]
    public enum UserRole
    {
        Member,
        Manager
    }
}
