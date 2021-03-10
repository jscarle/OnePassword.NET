using System;
using Newtonsoft.Json;

namespace OnePassword.Groups
{
    public class GroupTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((GroupType)value)
            {
                case GroupType.Owner:
                    writer.WriteValue("O");
                    break;
                case GroupType.Administrator:
                    writer.WriteValue("A");
                    break;
                case GroupType.Recovery:
                    writer.WriteValue("R");
                    break;
                case GroupType.TeamMember:
                    writer.WriteValue("M");
                    break;
                case GroupType.User:
                    writer.WriteValue("U");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "O":
                    return GroupType.Owner;
                case "A":
                    return GroupType.Administrator;
                case "R":
                    return GroupType.Recovery;
                case "M":
                    return GroupType.TeamMember;
                case "U":
                    return GroupType.User;
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
