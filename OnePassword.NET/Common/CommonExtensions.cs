namespace OnePassword.Common;

internal static class CommonExtensions
{
    internal static string ToEnumString<TField>(this TField field)
        where TField : Enum
    {
        var fieldInfo = typeof(TField).GetField(field.ToString());
        if (fieldInfo is null)
            throw new ArgumentException("Could not find enum field.", nameof(field));

        var attributes = (EnumMemberAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false);
        if (attributes.Length == 0)
            throw new NotImplementedException($"{nameof(EnumMemberAttribute)} has not been defined for this field.");

        var iconName = attributes[0].Value;
        if (iconName is null)
            throw new NotImplementedException($"{nameof(EnumMemberAttribute)} has not been defined for this field.");

        return iconName;
    }
}