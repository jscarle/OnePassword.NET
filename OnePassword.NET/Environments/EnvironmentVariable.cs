namespace OnePassword.Environments;

/// <summary>Represents a variable from a 1Password Environment.</summary>
public sealed class EnvironmentVariable
{
    /// <summary>The variable name.</summary>
    public string Name { get; internal set; } = "";

    /// <summary>The variable value.</summary>
    public string Value { get; internal set; } = "";

    /// <summary>Initializes a new instance of <see cref="EnvironmentVariable" />.</summary>
    public EnvironmentVariable()
    {
    }

    /// <summary>Initializes a new instance of <see cref="EnvironmentVariable" /> with the specified name and value.</summary>
    /// <param name="name">The variable name.</param>
    /// <param name="value">The variable value.</param>
    public EnvironmentVariable(string name, string value)
    {
        Name = name ?? "";
        Value = value ?? "";
    }

    /// <inheritdoc />
    public override string ToString() => $"{Name}={Value}";
}
