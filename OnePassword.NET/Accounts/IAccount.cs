namespace OnePassword.Accounts;

/// <summary>
/// Defines a 1Password account.
/// </summary>
public interface IAccount : IEquatable<IAccount>, IComparable<IAccount>, IComparable
{
    /// <summary>
    /// The account ID.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Deconstructs the account into its ID and name.
    /// </summary>
    /// <param name="id">The account ID.</param>
    /// <param name="name">The account name.</param>
    void Deconstruct(out string id, out string name);
}