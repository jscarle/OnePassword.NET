namespace OnePassword;

public interface IOnePasswordManagerOptions
{
    /// <summary>
    /// The path to the 1Password CLI executable. Defaults to the current working directory.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The name of the 1Password CLI executable. Defaults to 'op.exe'.
    /// </summary>
    public string Executable { get; set; }

    /// <summary>
    /// When <see langword="true" />, commands sent to the 1Password CLI executable are output to the console. Defaults to
    /// <see langword="false" />.
    /// </summary>
    public bool Verbose { get; set; }

    /// <summary>
    /// Set to <see langword="true" /> when authentication is integrated into the 1Password desktop application (see
    /// <a href="https://developer.1password.com/docs/cli/get-started/#sign-in">documentation</a>). When
    /// <see langword="false" />, a password or service account token will be required to sign in. Defaults to
    /// <see langword="false" />.
    /// </summary>
    public bool AppIntegrated { get; set; }

    /// <summary>
    /// The service account token. If a token is provided, login will not be required.
    /// </summary>
    public string ServiceAccountToken { get; set; }
}