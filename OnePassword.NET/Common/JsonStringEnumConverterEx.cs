﻿namespace OnePassword.Common;

/// <summary>
/// Converts between an enum and its string value.
/// </summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
/// <remarks>Originally authored by JasonBodley (https://github.com/JasonBodley) [https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673]</remarks>
internal sealed class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = new();
    private readonly Dictionary<string, TEnum> _stringToEnum = new();

    public JsonStringEnumConverterEx()
    {
        foreach (var enumMemberValue in Enum.GetValues<TEnum>())
        {
            var enumMemberName = enumMemberValue.ToString();
            var enumMemberAttribute = typeof(TEnum).GetMember(enumMemberName).FirstOrDefault()?.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();

            if (enumMemberAttribute is { Value: not null })
            {
                var enumMemberString = enumMemberAttribute.Value.ToUpperInvariant().Replace(" ", "_", StringComparison.InvariantCulture);
                _enumToString.Add(enumMemberValue, enumMemberString);
                _stringToEnum.Add(enumMemberString, enumMemberValue);
            }
            else
            {
                var enumMemberString = enumMemberName.ToUpperInvariant();
                _enumToString.Add(enumMemberValue, enumMemberString);
                _stringToEnum.Add(enumMemberString, enumMemberValue);
            }
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString() ?? "Unknown";

        var enumMemberString = stringValue.ToUpperInvariant().Replace(" ", "_", StringComparison.InvariantCulture);
        if (_stringToEnum.TryGetValue(enumMemberString, out var enumValue))
            return enumValue;

        throw new NotImplementedException("Could not convert string value to its enum representation.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_enumToString.TryGetValue(value, out var enumValue))
            writer.WriteStringValue(enumValue);
        else
            throw new NotImplementedException("Enum does not have its string representation defined.");
    }
}