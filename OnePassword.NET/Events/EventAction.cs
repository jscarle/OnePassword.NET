using Newtonsoft.Json;

namespace OnePassword.Events
{
    [JsonConverter(typeof(EventActionConverter))]
    public enum EventAction
    {
        Created,
        Updated,
        UpdatedAttribute,
        UpdatedItem,
        Deleted,
        Granted,
        Revoked,
        Suspended,
        Reactivated
    }
}