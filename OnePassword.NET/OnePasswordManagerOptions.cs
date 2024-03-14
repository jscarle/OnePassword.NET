namespace OnePassword;

/// <summary>Represents the 1Password manager options.</summary>
public class OnePasswordManagerOptions : IOnePasswordManagerOptions
{
    /// <summary>The default options.</summary>
    public static OnePasswordManagerOptions Default => new();

    /// <inheritdoc />
    public string Path { get; set; } = "";

    /// <inheritdoc />
    public string Executable { get; set; } = "op.exe";

    /// <inheritdoc />
    public bool Verbose { get; set; }

    /// <inheritdoc />
    public bool AppIntegrated { get; set; }

    /// <inheritdoc />
    public string ServiceAccountToken { get; set; } = "";
}
