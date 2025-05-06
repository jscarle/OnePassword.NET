using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace OnePassword.Common;

/// <summary>Common extensions methods.</summary>
internal static class CommonExtensions
{
    /// <summary>Converts an enum field to its string representation.</summary>
    /// <param name="field">The enum field.</param>
    /// <typeparam name="TField">The type of enum field.</typeparam>
    /// <returns>A string representation of the enum field.</returns>
    /// <exception cref="ArgumentNullException">Thrown when field is null.</exception>
    /// <exception cref="NotImplementedException">Thrown when field is not correctly annotated with a <see cref="EnumMemberAttribute" />.</exception>
    [SuppressMessage("AssemblyLoadTrimming", "IL2090:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to 'target method'.",
        Justification = "https://github.com/dotnet/runtime/issues/97737")]
    internal static string ToEnumString<TField>(this TField field) where TField : Enum
    {
        var fieldInfo = typeof(TField).GetField(field.ToString(), BindingFlags.Public | BindingFlags.Static) ?? throw new ArgumentNullException(nameof(field));
        var attributes = (EnumMemberAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);
        if (attributes.Length == 0)
            throw new NotImplementedException($"The field has not been annotated with a {nameof(EnumMemberAttribute)}.");

        var value = attributes[0].Value ?? throw new NotImplementedException($"{nameof(EnumMemberAttribute)}.{nameof(EnumMemberAttribute.Value)} has not been set for this field.");
        return value;
    }

    /// <summary>Converts enum fields to a comma separated list.</summary>
    /// <param name="fields">The enum fields.</param>
    /// <param name="replaceUnderscoresWithSpaces">When <see langword="true" />, replaces underscores in the field name with spaces.</param>
    /// <typeparam name="TField">The type of enum field.</typeparam>
    /// <returns>A comma separated list of the enum fields.</returns>
    internal static string ToCommaSeparated<TField>(this IEnumerable<TField> fields, bool replaceUnderscoresWithSpaces = false) where TField : struct, Enum
    {
        var values = fields.Select(field => field.ToEnumString()).ToList();
        var commaSeparated = string.Join(",", values);
        return replaceUnderscoresWithSpaces ? commaSeparated.Replace("_", " ", StringComparison.InvariantCulture) : commaSeparated;
    }

    /// <summary>Converts string items to a comma separated list.</summary>
    /// <param name="items">The string items.</param>
    /// <param name="replaceUnderscoresWithSpaces">When <see langword="true" />, replaces underscores in the field name with spaces.</param>
    /// <returns>A comma separated list of the string items.</returns>
    internal static string ToCommaSeparated(this IEnumerable<string> items, bool replaceUnderscoresWithSpaces = false)
    {
        var commaSeparated = string.Join(",", items);
        return replaceUnderscoresWithSpaces ? commaSeparated.Replace("_", " ", StringComparison.InvariantCulture) : commaSeparated;
    }

    /// <summary>Converts a <see cref="TimeSpan" /> to a human readable string.</summary>
    /// <returns>A human readable string representing the time span.</returns>
    internal static string ToHumanReadable(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds < 1)
            return "0s";

        var result = "";
        if (timeSpan.Days > 7)
        {
            result += $"{timeSpan.Days / 7}w";
            timeSpan = TimeSpan.FromDays(timeSpan.Days % 7);
        }

        if (timeSpan.Days > 0)
            result += $"{timeSpan.Days}d";

        if (timeSpan.Hours > 0)
            result += $"{timeSpan.Hours}h";

        if (timeSpan.Minutes > 0)
            result += $"{timeSpan.Minutes}m";

        if (timeSpan.Seconds > 0)
            result += $"{timeSpan.Seconds}s";

        return result.Length == 0 ? "0s" : result;
    }
}