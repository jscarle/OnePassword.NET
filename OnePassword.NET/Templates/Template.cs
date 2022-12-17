using OnePassword.Items;

namespace OnePassword.Templates;

public sealed class Template : ItemBase, ITemplate
{
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    public Template Clone()
    {
        var json = JsonSerializer.Serialize(this);
        return JsonSerializer.Deserialize<Template>(json) ?? throw new SerializationException("Could not deserialize the cloned template.");
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(Template a, ITemplate b) => a.Equals(b);

    public static bool operator !=(Template a, ITemplate b) => !a.Equals(b);

    public static bool operator <(Template a, ITemplate b) => a.CompareTo(b) < 0;

    public static bool operator <=(Template a, ITemplate b) => a.CompareTo(b) <= 0;

    public static bool operator >(Template a, ITemplate b) => a.CompareTo(b) > 0;

    public static bool operator >=(Template a, ITemplate b) => a.CompareTo(b) >= 0;

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ITemplate other && Equals(other);
    }

    public bool Equals(ITemplate? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is ITemplate other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ITemplate)}");
    }

    public int CompareTo(ITemplate? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        // Name can only be set by internal methods.
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }
}