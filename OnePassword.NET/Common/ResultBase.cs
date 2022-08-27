namespace OnePassword.Common;

public abstract class ResultBase<TInterface> : IResultBase<TInterface>
    where TInterface : IResultBase<TInterface>
{
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal init; } = "";

    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Name;
    }

    public override string ToString()
    {
        return Name;
    }

    public static bool operator ==(ResultBase<TInterface> a, IResultBase<TInterface> b) => a.Equals(b);
    public static bool operator !=(ResultBase<TInterface> a, IResultBase<TInterface> b) => !a.Equals(b);

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is IResultBase<TInterface> other && Equals(other);
    }

    public bool Equals(IResultBase<TInterface>? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is TInterface other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(TInterface)}");
    }

    public int CompareTo(IResultBase<TInterface>? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Id);
    }
}