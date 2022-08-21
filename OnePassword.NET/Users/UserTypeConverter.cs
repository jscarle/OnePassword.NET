using System;
using Newtonsoft.Json;

namespace OnePassword.Users
{
    public class UserTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((UserType)value)
            {
                case UserType.Regular:
                    writer.WriteValue("R");
                    break;
                case UserType.Guest:
                    writer.WriteValue("G");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "R":
                    return UserType.Regular;
                case "G":
                    return UserType.Guest;
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
