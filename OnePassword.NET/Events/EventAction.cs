using Newtonsoft.Json;

namespace OnePassword.Events
{
    [JsonConverter(typeof(EventActionConverter))]
    public enum EventAction
    {
        Created,
        Modified,
        Updated,
        UpdatedAccess,
        Deleted,
        Granted,
        Revoked,
        Suspended,
        Reactivated
    }
}