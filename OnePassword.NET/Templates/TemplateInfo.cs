namespace OnePassword.Templates;

/// <summary>Describes a 1Password template.</summary>
public sealed class TemplateInfo : ITemplate
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("uuid")]
    public string UUID { get; internal set; } = "";

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    /// <inheritdoc />
    public override string ToString() => $"{Name} ({UUID})";

    /// <summary>Equality operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator ==(TemplateInfo? a, ITemplate? b) => Equals(a, b);

    /// <summary>Inequality operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator !=(TemplateInfo? a, ITemplate? b) => !Equals(a, b);

    /// <summary>Less than operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is less than <paramref name="b" />; otherwise, false.</returns>
    public static bool operator <(TemplateInfo? a, ITemplate? b) => NullSafeCompareTo(a, b) < 0;

    /// <summary>Less than or equal to operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is less than or equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator <=(TemplateInfo? a, ITemplate? b) => NullSafeCompareTo(a, b) <= 0;

    /// <summary>Greater than operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is greater than <paramref name="b" />; otherwise, false.</returns>
    public static bool operator >(TemplateInfo? a, ITemplate? b) => NullSafeCompareTo(a, b) > 0;

    /// <summary>Greater than or equal to operator.</summary>
    /// <param name="a">The <see cref="TemplateInfo" /> object.</param>
    /// <param name="b">The <see cref="ITemplate" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is greater than or equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator >=(TemplateInfo? a, ITemplate? b) => NullSafeCompareTo(a, b) >= 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ITemplate other && Equals(other);

    /// <inheritdoc />
    public bool Equals(ITemplate? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(UUID, other.UUID, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is ITemplate other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ITemplate)}");
    }

    /// <inheritdoc />
    public int CompareTo(ITemplate? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(UUID, other.UUID, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // Name can only be set by internal methods.
        StringComparer.OrdinalIgnoreCase.GetHashCode(UUID) + StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

    private static int NullSafeCompareTo(TemplateInfo? a, ITemplate? b)
    {
        if (a is not null)
            return b is null ? 1 : a.CompareTo(b);
        if (b is null)
            return 0;
        return -1;
    }
}
