namespace OnePassword.Common;

/// <summary>
/// Converts between an enum and its string value.
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
/// <remarks>Originally authored by JasonBodley (https://github.com/JasonBodley) [https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673]</remarks>
internal class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum> where TEnum : struct, System.Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = new();
    private readonly Dictionary<string, TEnum> _stringToEnum = new();

    public JsonStringEnumConverterEx()
    {
        var type = typeof(TEnum);
        var values = Enum.GetValues<TEnum>();

        foreach (var value in values)
        {
            var enumMember = type.GetMember(value.ToString())[0];
            var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();

            _stringToEnum.Add(value.ToString(), value);

            if (attr is { Value: not null })
            {
                _enumToString.Add(value, attr.Value.ToUpper());
                _stringToEnum.Add(attr.Value.ToUpper(), value);
            }
            else
            {
                _enumToString.Add(value, value.ToString());
            }
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        if (stringValue is not null && _stringToEnum.TryGetValue(stringValue.ToUpper(), out var enumValue))
            return enumValue;

        throw new NotImplementedException("Could not convert string value to its enum representation.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_enumToString.ContainsKey(value))
            writer.WriteStringValue(_enumToString[value]);
        else
            throw new NotImplementedException("Enum does not have its string representation defined.");
    }
}