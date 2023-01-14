namespace OnePassword.Common;

/// <summary>
/// Defines a command result.
/// </summary>
/// <typeparam name="T">The result type.</typeparam>
public interface IResult<T> : IEquatable<IResult<T>>, IComparable<IResult<T>>, IComparable
{
    /// <summary>
    /// The object ID.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The object name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Deconstructs the object into its ID and name. 
    /// </summary>
    /// <param name="id">The object ID.</param>
    /// <param name="name">The object name.</param>
    void Deconstruct(out string id, out string name);
}