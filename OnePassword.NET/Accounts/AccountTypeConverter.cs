using System;
using Newtonsoft.Json;

namespace OnePassword.Accounts
{
    public class AccountTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((AccountType)value)
            {
                case AccountType.Personal:
                    writer.WriteValue("P");
                    break;
                case AccountType.Business:
                    writer.WriteValue("B");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "P":
                    return AccountType.Personal;
                case "B":
                    return AccountType.Business;
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
