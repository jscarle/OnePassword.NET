using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace OnePassword.Common;

/// <summary>Converts between an enum and its string value.</summary>
/// <typeparam name="TEnum">The enum type.</typeparam>
/// <remarks>Originally authored by JasonBodley (https://github.com/JasonBodley) [https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673]</remarks>
[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
internal sealed class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = [];
    private readonly Dictionary<string, TEnum> _stringToEnum = [];

    /// <summary>Initializes a new instance of <see cref="JsonStringEnumConverterEx{TEnum}" />.</summary>
    [SuppressMessage("AssemblyLoadTrimming", "IL2090:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to 'target method'.", Justification = "https://github.com/dotnet/runtime/issues/97737")]
    public JsonStringEnumConverterEx()
    {
        foreach (TEnum enumMemberValue in Enum.GetValues(typeof(TEnum)))
        {
            var enumMemberName = enumMemberValue.ToString();

            var enumMemberAttribute = typeof(TEnum).GetMember(enumMemberName, BindingFlags.Public | BindingFlags.Static).FirstOrDefault()?.GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();
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

    /// <inheritdoc />
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString() ?? "Unknown";

        var enumMemberString = stringValue.ToUpperInvariant().Replace(" ", "_", StringComparison.InvariantCulture);
        if (_stringToEnum.TryGetValue(enumMemberString, out var enumValue))
            return enumValue;

        throw new NotImplementedException("Could not convert string value to its enum representation.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (_enumToString.TryGetValue(value, out var enumValue))
            writer.WriteStringValue(enumValue);
        else
            throw new NotImplementedException("Enum does not have its string representation defined.");
    }
}
