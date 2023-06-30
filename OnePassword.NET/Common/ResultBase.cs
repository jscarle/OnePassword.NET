namespace OnePassword.Common;

/// <summary>
/// Common base class that represents a command result.
/// </summary>
/// <typeparam name="TInterface">The result interface type.</typeparam>
public abstract class ResultBase<TInterface> : IResult<TInterface>
    where TInterface : IResult<TInterface>
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal init; } = "";

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal init; } = "";

    /// <inheritdoc />
    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Name;
    }

    /// <inheritdoc />
    public bool Equals(IResult<TInterface>? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is TInterface other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(TInterface)}");
    }

    /// <inheritdoc />
    public int CompareTo(IResult<TInterface>? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    public static bool operator ==(ResultBase<TInterface> a, IResult<TInterface> b) => Equals(a, b);

    public static bool operator !=(ResultBase<TInterface> a, IResult<TInterface> b) => !Equals(a, b);

    public static bool operator <(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) < 0;

    public static bool operator <=(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) <= 0;

    public static bool operator >(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) > 0;

    public static bool operator >=(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) >= 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is IResult<TInterface> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Id);

    private static int NullSafeCompareTo(IComparable? a, object? b)
    {
        if (a is not null)
            return b is null ? 1 : a.CompareTo(b);
        if (b is null)
            return 0;
        return -1;
    }
}