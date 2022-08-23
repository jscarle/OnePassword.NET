namespace OnePassword.Common;

internal static class CommonExtensions
{
    internal static string ToStringEnum<TEnum>(this TEnum enumField)
        where TEnum : struct, Enum
    {
        var field = typeof(TEnum).GetField(enumField.ToString());
        if (field is null)
            throw new ArgumentException("Could not find enum field.", nameof(enumField));

        var attributes = (EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
        if (attributes.Length == 0)
            throw new NotImplementedException($"{nameof(EnumMemberAttribute)} has not been defined for this field.");

        var iconName = attributes[0].Value;
        if (iconName is null)
            throw new NotImplementedException($"{nameof(EnumMemberAttribute)} has not been defined for this field.");

        return iconName;
    }
}