using System;
using Newtonsoft.Json;

namespace OnePassword.Vaults
{
    public class VaultTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((VaultType)value)
            {
                case VaultType.Personal:
                    writer.WriteValue("P");
                    break;
                case VaultType.User:
                    writer.WriteValue("U");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "P":
                    return VaultType.Personal;
                case "U":
                    return VaultType.User;
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
