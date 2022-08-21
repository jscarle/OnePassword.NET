using System;
using Newtonsoft.Json;

namespace OnePassword.Common
{
    public class YesNoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((bool)value)
            {
                case true:
                    writer.WriteValue("Y");
                    break;
                case false:
                    writer.WriteValue("N");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "Y":
                    return true;
                case "N":
                    return false;
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
