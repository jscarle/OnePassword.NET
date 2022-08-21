using System;
using Newtonsoft.Json;

namespace OnePassword.Events
{
    public class ObjectTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch ((ObjectType)value)
            {
                case ObjectType.Item:
                    writer.WriteValue("items");
                    break;
                case ObjectType.File:
                    writer.WriteValue("file");
                    break;
                case ObjectType.Device:
                    writer.WriteValue("device");
                    break;
                case ObjectType.User:
                    writer.WriteValue("user");
                    break;
                case ObjectType.UserVaultAccess:
                    writer.WriteValue("uva");
                    break;
                case ObjectType.GroupVaultAccess:
                    writer.WriteValue("gva");
                    break;
                case ObjectType.Vault:
                    writer.WriteValue("vault");
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch ((string)reader.Value)
            {
                case "items":
                    return ObjectType.Item;
                case "file":
                    return ObjectType.File;
                case "device":
                    return ObjectType.Device;
                case "user":
                    return ObjectType.User;
                case "uva":
                    return ObjectType.UserVaultAccess;
                case "gva":
                    return ObjectType.GroupVaultAccess;
                case "vault":
                    return ObjectType.Vault;
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
