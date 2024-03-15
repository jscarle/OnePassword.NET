namespace OnePassword.Common;

/// <summary>Common base class that represents a command result.</summary>
/// <typeparam name="TInterface">The result interface type.</typeparam>
public abstract class ResultBase<TInterface> : IResult<TInterface> where TInterface : IResult<TInterface>
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("id")]
    public string Id { get; internal set; } = "";

    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; internal set; } = "";

    /// <inheritdoc />
    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Name;
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
    public bool Equals(IResult<TInterface>? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is IResult<TInterface> other && Equals(other);

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <summary>Equality operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator ==(ResultBase<TInterface> a, IResult<TInterface> b) => Equals(a, b);

    /// <summary>Inequality operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator !=(ResultBase<TInterface> a, IResult<TInterface> b) => !Equals(a, b);

    /// <summary>Less than operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is less than <paramref name="b" />; otherwise, false.</returns>
    public static bool operator <(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) < 0;

    /// <summary>Less than or equal to operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is less than or equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator <=(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) <= 0;

    /// <summary>Greater than operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is greater than <paramref name="b" />; otherwise, false.</returns>
    public static bool operator >(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) > 0;

    /// <summary>Greater than or equal to operator.</summary>
    /// <param name="a">The <see cref="ResultBase{TInterface}" /> object.</param>
    /// <param name="b">The <see cref="IResult{TInterface}" /> object to compare.</param>
    /// <returns>True if the <paramref name="a" /> is greater than or equal to <paramref name="b" />; otherwise, false.</returns>
    public static bool operator >=(ResultBase<TInterface> a, IResult<TInterface> b) => NullSafeCompareTo(a, b) >= 0;

    /// <inheritdoc />
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Id);

    private static int NullSafeCompareTo(ResultBase<TInterface>? a, object? b)
    {
        if (a is not null)
            return b is null ? 1 : a.CompareTo(b);
        if (b is null)
            return 0;
        return -1;
    }
}
