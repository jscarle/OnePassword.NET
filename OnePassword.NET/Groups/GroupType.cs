using Newtonsoft.Json;

namespace OnePassword.Groups
{
    [JsonConverter(typeof(GroupTypeConverter))]
    public enum GroupType
    {
        Owner,
        Administrator,
        Recovery,
        TeamMember,
        User
    }
}
