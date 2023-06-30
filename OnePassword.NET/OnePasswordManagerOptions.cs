namespace OnePassword;

public class OnePasswordManagerOptions : IOnePasswordManagerOptions
{
    public static readonly OnePasswordManagerOptions Default = new();

    /// <inheritdoc />
    public string Path { get; set; } = "";

    /// <inheritdoc />
    public string Executable { get; set; } = "op.exe";

    /// <inheritdoc />
    public bool Verbose { get; set; } = false;

    /// <inheritdoc />
    public bool AppIntegrated { get; set; } = false;

    /// <inheritdoc />
    public string ServiceAccountToken { get; set; } = "";
}