using System.Diagnostics.CodeAnalysis;

namespace OnePassword;

/// <summary>Defines a 1Password CLI executable manager.</summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public partial interface IOnePasswordManager
{
    /// <summary>The version of the 1Password CLI executable.</summary>
    public string Version { get; }

    /// <summary>Updates the 1Password CLI executable.</summary>
    /// <returns>Returns <see langword="true" /> when the 1Password CLI executable has been updated, <see langword="false" /> otherwise.</returns>
    public bool Update();

    /// <summary>Gets a secret.</summary>
    /// <returns>Returns the secret.</returns>
    /// <param name="reference">The reference to the secret.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public string GetSecret(string reference);

    /// <summary>Saves a secret.</summary>
    /// <param name="reference">The reference to the secret.</param>
    /// <param name="filePath">The file path to save the document to.</param>
    /// <param name="fileMode">The file mode.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SaveSecret(string reference, string filePath, string? fileMode = null);
}
