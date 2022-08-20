namespace OnePassword.Common;

public class YesNoJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() switch
        {
            "Y" => true,
            _ => false
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case true:
                writer.WriteStringValue("Y");
                break;
            case false:
                writer.WriteStringValue("N");
                break;
        }
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(string);
    }
}