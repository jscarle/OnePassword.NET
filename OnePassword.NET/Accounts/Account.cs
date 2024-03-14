using System.Diagnostics.CodeAnalysis;

namespace OnePassword.Accounts;

/// <summary>Represents a 1Password account.</summary>
public sealed class Account : IAccount
{
    /// <inheritdoc />
    [JsonInclude]
    [JsonPropertyName("account_uuid")]
    public string Id { get; internal init; } = "";

    /// <summary>The account shorthand.</summary>
    [JsonInclude]
    [JsonPropertyName("shorthand")]
    public string Shorthand { get; internal init; } = "";

    /// <summary>The account URL.</summary>
    [JsonInclude]
    [JsonPropertyName("url")]
    [SuppressMessage("Design", "CA1056:URI-like properties should not be strings")]
    public string Url { get; internal init; } = "";

    /// <summary>The user ID for the user associated with the account.</summary>
    [JsonInclude]
    [JsonPropertyName("user_uuid")]
    public string UserId { get; internal init; } = "";

    /// <summary>The email address for the user associated with the account.</summary>
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
    public override string ToString() => Shorthand;

    /// <summary>Equality operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the accounts are equal, false otherwise.</returns>
    public static bool operator ==(Account a, IAccount b) => a?.Equals(b) ?? false;

    /// <summary>Inequality operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the accounts are not equal, false otherwise.</returns>
    public static bool operator !=(Account a, IAccount b) => !a?.Equals(b) ?? false;

    /// <summary>Less than operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the first account is less than the second one, false otherwise.</returns>
    public static bool operator <(Account a, IAccount b) => a?.CompareTo(b) < 0;

    /// <summary>Less than or equal to operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the first account is less than or equal to the second one, false otherwise.</returns>
    public static bool operator <=(Account a, IAccount b) => a?.CompareTo(b) <= 0;

    /// <summary>Greater than operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the first account is greater than the second one, false otherwise.</returns>
    public static bool operator >(Account a, IAccount b) => a?.CompareTo(b) > 0;

    /// <summary>Greater than or equal to operator.</summary>
    /// <param name="a">The first account to compare.</param>
    /// <param name="b">The second account to compare.</param>
    /// <returns>True if the first account is greater than or equal to the second one, false otherwise.</returns>
    public static bool operator >=(Account a, IAccount b) => a?.CompareTo(b) >= 0;

    /// <inheritdoc />
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is IAccount other && Equals(other);

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

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Id);

    private int CompareTo(Account? other)
    {
        if (other is null) return 1;
        return ReferenceEquals(this, other) ? 0 : string.Compare(Shorthand, other.Shorthand, StringComparison.Ordinal);
    }

    private int CompareTo(AccountDetails? other) => other is null ? 1 : string.Compare(Shorthand, other.Domain, StringComparison.Ordinal);
}
