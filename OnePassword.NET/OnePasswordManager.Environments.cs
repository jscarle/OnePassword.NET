using OnePassword.Environments;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<EnvironmentVariable> GetEnvironmentVariables(string environmentId)
    {
        var result = ReadEnvironmentVariables(environmentId);
        return ParseEnvironmentVariables(result);
    }

    /// <inheritdoc />
    public void SaveEnvironmentVariables(string environmentId, string filePath, string? fileMode = null)
    {
        ValidateEnvironmentId(environmentId);
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        var trimmedFileMode = fileMode?.Trim();
        var result = ReadEnvironmentVariables(environmentId);

        File.WriteAllText(trimmedFilePath, result);
        if (trimmedFileMode is not null && trimmedFileMode.Length > 0)
            ApplyFileMode(trimmedFileMode, trimmedFilePath);
    }

    private string ReadEnvironmentVariables(string environmentId)
    {
        ValidateEnvironmentId(environmentId);
        var trimmedEnvironmentId = environmentId.Trim();
        var command = $"environment read {trimmedEnvironmentId}";
        return Op(command, Array.Empty<string>(), false, false);
    }

    private static ImmutableList<EnvironmentVariable> ParseEnvironmentVariables(string result)
    {
        var variables = ImmutableList.CreateBuilder<EnvironmentVariable>();
        using var reader = new StringReader(result);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            if (line.Length == 0)
                continue;

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
                throw new SerializationException("Could not deserialize the command result.");

            variables.Add(new EnvironmentVariable(line[..separatorIndex], line[(separatorIndex + 1)..]));
        }

        return variables.ToImmutable();
    }

    private static void ValidateEnvironmentId(string environmentId)
    {
        if (environmentId is null || environmentId.Length == 0)
            throw new ArgumentException($"{nameof(environmentId)} cannot be empty.", nameof(environmentId));
        var trimmedEnvironmentId = environmentId.Trim();
        if (trimmedEnvironmentId.Length == 0)
            throw new ArgumentException($"{nameof(trimmedEnvironmentId)} cannot be empty.", nameof(environmentId));
    }
}
