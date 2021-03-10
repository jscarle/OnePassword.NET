using System;
using Newtonsoft.Json;

namespace OnePassword.Events
{
    public class EventActionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((EventAction)value)
            {
                case EventAction.Created:
                    writer.WriteValue("create");
                    break;
                case EventAction.Modified:
                    writer.WriteValue("patch");
                    break;
                case EventAction.Updated:
                    writer.WriteValue("update");
                    break;
                case EventAction.UpdatedAccess:
                    writer.WriteValue("updatea");
                    break;
                case EventAction.Deleted:
                    writer.WriteValue("delete");
                    break;
                case EventAction.Granted:
                    writer.WriteValue("grant");
                    break;
                case EventAction.Revoked:
                    writer.WriteValue("revoke");
                    break;
                case EventAction.Suspended:
                    writer.WriteValue("suspend");
                    break;
                case EventAction.Reactivated:
                    writer.WriteValue("reactive");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "create":
                    return EventAction.Created;
                case "patch":
                    return EventAction.Modified;
                case "update":
                    return EventAction.Updated;
                case "updatea":
                    return EventAction.UpdatedAccess;
                case "delete":
                    return EventAction.Deleted;
                case "grant":
                    return EventAction.Granted;
                case "revoke":
                    return EventAction.Revoked;
                case "suspend":
                    return EventAction.Suspended;
                case "reactive":
                    return EventAction.Reactivated;
                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
