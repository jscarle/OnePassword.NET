namespace OnePassword.Templates;

/// <summary>
/// Defines a 1Password template.
/// </summary>
public interface ITemplate : IEquatable<ITemplate>, IComparable<ITemplate>, IComparable
{
    /// <summary>
    /// The template UUID.
    /// </summary>
    string Uuid { get; }

    /// <summary>
    /// The template name.
    /// </summary>
    string Name { get; }
}