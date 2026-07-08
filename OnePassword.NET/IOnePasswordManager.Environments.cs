using OnePassword.Environments;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the environment variables for a 1Password Environment.</summary>
    /// <param name="environmentId">The Environment ID.</param>
    /// <returns>The environment variables.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<EnvironmentVariable> GetEnvironmentVariables(string environmentId);

    /// <summary>Saves the environment variables for a 1Password Environment to disk.</summary>
    /// <param name="environmentId">The Environment ID.</param>
    /// <param name="filePath">The output file path.</param>
    /// <param name="fileMode">The file mode to use when creating the file.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SaveEnvironmentVariables(string environmentId, string filePath, string? fileMode = null);
}
