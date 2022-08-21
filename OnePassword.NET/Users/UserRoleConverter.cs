using System;
using Newtonsoft.Json;

namespace OnePassword.Users
{
    public class UserRoleConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((UserRole)value)
            {
                case UserRole.Member:
                    writer.WriteValue("member");
                    break;
                case UserRole.Manager:
                    writer.WriteValue("manager");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "member":
                    return UserRole.Member;
                case "manager":
                    return UserRole.Manager;
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
