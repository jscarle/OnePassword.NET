namespace OnePassword.Templates;

/// <summary>
/// Describes a 1Password template.
/// </summary>
public sealed class TemplateInfo : ITemplate
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(TemplateInfo a, ITemplate b) => a.Equals(b);

    public static bool operator !=(TemplateInfo a, ITemplate b) => !a.Equals(b);

    public static bool operator <(TemplateInfo a, ITemplate b) => a.CompareTo(b) < 0;

    public static bool operator <=(TemplateInfo a, ITemplate b) => a.CompareTo(b) <= 0;

    public static bool operator >(TemplateInfo a, ITemplate b) => a.CompareTo(b) > 0;

    public static bool operator >=(TemplateInfo a, ITemplate b) => a.CompareTo(b) >= 0;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ITemplate other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(ITemplate? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
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
        return ReferenceEquals(this, other) ? 0 : string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // Name can only be set by internal methods.
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }
}