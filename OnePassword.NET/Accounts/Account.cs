namespace OnePassword.Accounts;

/// <summary>
/// Represents a 1Password account.
/// </summary>
public sealed class Account : IAccount
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("account_uuid")]
    public string Id { get; internal init; } = "";

    /// <summary>
    /// The account shorthand.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("shorthand")]
    public string Shorthand { get; internal init; } = "";

    /// <summary>
    /// The account URL.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("url")]
    public string Url { get; internal init; } = "";

    /// <summary>
    /// The user ID for the user associated with the account.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("user_uuid")]
    public string UserId { get; internal init; } = "";

    /// <summary>
    /// The email address for the user associated with the account.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("email")]
    public string Email { get; internal init; } = "";

    /// <inheritdoc />
    public void Deconstruct(out string id, out string name)
    {
        id = Id;
        name = Shorthand;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Shorthand;
    }

    public static bool operator ==(Account a, IAccount b) => a.Equals(b);

    public static bool operator !=(Account a, IAccount b) => !a.Equals(b);

    public static bool operator <(Account a, IAccount b) => a.CompareTo(b) < 0;

    public static bool operator <=(Account a, IAccount b) => a.CompareTo(b) <= 0;

    public static bool operator >(Account a, IAccount b) => a.CompareTo(b) > 0;

    public static bool operator >=(Account a, IAccount b) => a.CompareTo(b) >= 0;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is IAccount other && Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(IAccount? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is IAccount other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(IAccount)}");
    }

    /// <inheritdoc />
    public int CompareTo(IAccount? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;
        return other switch
        {
            Account account => CompareTo(account),
            AccountDetails accountDetails => CompareTo(accountDetails),
            _ => string.Compare(Id, other.Id, StringComparison.Ordinal)
        };
    }

    private int CompareTo(Account? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(Shorthand, other.Shorthand, StringComparison.Ordinal);
    }

    private int CompareTo(AccountDetails? other)
    {
        return other is null ? 1 : string.Compare(Shorthand, other.Domain, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Id);
    }
}