namespace OnePassword.Items;

/// <summary>
/// Defines a 1Password item.
/// </summary>
public interface IItem
{
    /// <summary>
    /// The item ID.
    /// </summary>
    string Id { get; }
}