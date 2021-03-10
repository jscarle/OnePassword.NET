using System;
using Newtonsoft.Json;

namespace OnePassword.Common
{
    public class StateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((State)value)
            {
                case State.Active:
                    writer.WriteValue("A");
                    break;
                case State.Suspended:
                    writer.WriteValue("S");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "A":
                    return State.Active;
                case "S":
                    return State.Suspended;
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
