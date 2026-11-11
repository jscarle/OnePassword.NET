using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using OnePassword.Common;

namespace OnePassword;

/// <summary>Represents the 1Password CLI executable manager.</summary>
public sealed partial class OnePasswordManager : IOnePasswordManager
{
    /// <inheritdoc />
    public string Version { get; private set; }

#if NET7_0_OR_GREATER
    private static readonly Regex VersionRegex = GeneratedVersionRegex();
#else
    private static readonly Regex VersionRegex = new (@"Version ([^\s]+) is now available\.", RegexOptions.Compiled);
#endif
    private readonly string[] _excludedAccountCommands = ["--version", "update", "account list", "account add", "account forget", "signout --all"];
    private readonly string[] _excludedSessionCommands = ["--version", "update", "account list", "account add", "account forget", "signin", "signout", "signout --all"];
    private readonly Mode _mode = Mode.Interactive;
    private readonly string _opPath;
    private readonly string _serviceAccountToken;
    private readonly string[] _serviceAccountUnsupportedCommands =
    [
        "connect", "events-api", "group create", "group delete", "group edit", "group user",
        "user provision", "user confirm", "user suspend", "user delete", "user recovery", "user reactivate", "user edit", "vault edit"
    ];
    private readonly bool _verbose;
    private string _account = "";
    private string _session = "";

    /// <summary>Initializes a new instance of <see cref="OnePasswordManager" /> using the specified options.</summary>
    /// <param name="options">The configuration options.</param>
    /// <exception cref="FileNotFoundException">Thrown when the 1Password CLI executable cannot be found.</exception>
    public OnePasswordManager(Action<OnePasswordManagerOptions> options) : this(ConfigureOptions(options))
    {
    }

    /// <summary>Initializes a new instance of <see cref="OnePasswordManager" /> using the specified options.</summary>
    /// <param name="options">The configuration options.</param>
    /// <exception cref="FileNotFoundException">Thrown when the 1Password CLI executable cannot be found.</exception>
    public OnePasswordManager(OnePasswordManagerOptions? options = null)
    {
        var configuration = ValidateOptions(options);
        var executable = string.IsNullOrWhiteSpace(configuration.Executable) ? OnePasswordManagerOptions.GetDefaultExecutableName() : configuration.Executable.Trim();

        _opPath = configuration.Path.Length > 0 ? Path.Combine(configuration.Path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
        if (!File.Exists(_opPath))
            throw new FileNotFoundException($"The 1Password CLI executable ({executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = configuration.Verbose;

        if (configuration.AppIntegrated)
            _mode = Mode.AppIntegrated;

        _serviceAccountToken = configuration.ServiceAccountToken;
        if (_serviceAccountToken.Length > 0)
            _mode = Mode.ServiceAccount;

        Version = GetVersion();
    }

    /// <inheritdoc />
    public bool Update()
    {
        var updated = false;
        var packagedExecutableName = OnePasswordManagerOptions.GetDefaultExecutableName();

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        try
        {
            var command = $"update --directory {QuoteArgument(tempDirectory)} --channel stable";
            var result = Op(command);

            var match = VersionRegex.Match(result);
            if (match.Success)
            {
                foreach (var file in Directory.GetFiles(tempDirectory, "*.zip"))
                {
                    using var zipArchive = ZipFile.Open(file, ZipArchiveMode.Read);

                    var entry = zipArchive.Entries.FirstOrDefault(zipEntry => string.Equals(Path.GetFileName(zipEntry.FullName), packagedExecutableName, StringComparison.OrdinalIgnoreCase));
                    if (entry is null)
                        continue;
                    entry.ExtractToFile(_opPath, true);
                    EnsureExecutablePermissions(_opPath);

                    Version = GetVersion();
                    updated = true;
                    break;
                }
            }
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }

        return updated;
    }

    /// <inheritdoc />
    public string GetSecret(string reference)
    {
        if (reference is null || reference.Length == 0)
            throw new ArgumentException($"{nameof(reference)} cannot be empty.", nameof(reference));
        var trimmedReference = reference.Trim();
        if (trimmedReference.Length == 0)
            throw new ArgumentException($"{nameof(trimmedReference)} cannot be empty.", nameof(reference));

        var command = $"read {QuoteArgument(trimmedReference)} --no-newline";
        return Op(command);
    }

    /// <inheritdoc />
    public void SaveSecret(string reference, string filePath, string? fileMode = null)
    {
        if (reference is null || reference.Length == 0)
            throw new ArgumentException($"{nameof(reference)} cannot be empty.", nameof(reference));
        var trimmedReference = reference.Trim();
        if (trimmedReference.Length == 0)
            throw new ArgumentException($"{nameof(trimmedReference)} cannot be empty.", nameof(reference));
        if (filePath is null || filePath.Length == 0)
            throw new ArgumentException($"{nameof(filePath)} cannot be empty.", nameof(filePath));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        var trimmedFileMode = fileMode?.Trim();
        var command = $"read {QuoteArgument(trimmedReference)} --no-newline --force --out-file {QuoteArgument(trimmedFilePath)}";
        if (trimmedFileMode is not null && trimmedFileMode.Length > 0)
            command += $" --file-mode {QuoteArgument(trimmedFileMode)}";
        Op(command);
    }

    private static OnePasswordManagerOptions ConfigureOptions(Action<OnePasswordManagerOptions>? configure)
    {
        if (configure is null)
            return OnePasswordManagerOptions.Default;
        var options = OnePasswordManagerOptions.Default;
        configure(options);
        return options;
    }

    private static OnePasswordManagerOptions ValidateOptions(OnePasswordManagerOptions? options)
    {
        if (options is { AppIntegrated: true, ServiceAccountToken.Length: > 0 })
            throw new InvalidOperationException("Cannot use a service account token when running in app integrated mode.");

        return options ?? OnePasswordManagerOptions.Default;
    }

    private string GetVersion()
    {
        const string command = "--version";
        return Op(command).Trim();
    }

    private static void EnsureExecutablePermissions(string executablePath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        using var chmod = Process.Start(new ProcessStartInfo("chmod", $"+x \"{executablePath}\"")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });
        chmod?.WaitForExit();
    }

    private static string QuoteArgument(string argument)
    {
        var quotedArgument = new StringBuilder(argument.Length + 2);
        quotedArgument.Append('"');
        var backslashCount = 0;

        foreach (var character in argument)
        {
            if (character == '\\')
            {
                backslashCount++;
                continue;
            }

            // Backslashes before a quote, including the closing quote, must be doubled.
            quotedArgument.Append('\\', character == '"' ? backslashCount * 2 + 1 : backslashCount);
            quotedArgument.Append(character);
            backslashCount = 0;
        }

        quotedArgument.Append('\\', backslashCount * 2);
        quotedArgument.Append('"');
        return quotedArgument.ToString();
    }

    private TResult Op<TResult>(JsonTypeInfo<TResult> jsonTypeInfo, string command, string? input = null, string? account = null) where TResult : class
    {
        var result = Op(command, input is null ? Array.Empty<string>() : [input], account: account);
        var obj = JsonSerializer.Deserialize(result, jsonTypeInfo) ?? throw new SerializationException("Could not deserialize the command result.");
        if (obj is ITracked item)
            item.AcceptChanges();
        return obj;
    }

    private string Op(string command, string? input = null, bool allowDeviceRegistration = false, string? secretKey = null) =>
        Op(command, input is null ? Array.Empty<string>() : [input], allowDeviceRegistration, secretKey: secretKey);

    private string Op(string command, IEnumerable<string> input, bool allowDeviceRegistration = false, string? account = null, string? secretKey = null)
    {
        var arguments = command;
        if (command != "--version")
            arguments += " --format json --no-color";

        switch (_mode)
        {
            case Mode.ServiceAccount:
                if (IsUnsupportedCommand(command, _serviceAccountUnsupportedCommands))
                    throw new InvalidOperationException($"Unsupported command {command} when using ServiceAccount");
                break;
            case Mode.Interactive:
            case Mode.AppIntegrated:
            default:
                var excluded = IsExcludedCommand(command, _excludedAccountCommands);
                var requireAccount = _mode != Mode.AppIntegrated && !excluded;
                var selectedAccount = account ?? _account;
                var passAccount = selectedAccount.Length != 0 && !excluded;
                if (requireAccount && !passAccount)
                    throw new InvalidOperationException("Cannot execute command because account has not been set.");

                var passSession = !(_mode == Mode.AppIntegrated || IsExcludedCommand(command, _excludedSessionCommands));
                if (passSession && _session.Length == 0)
                    throw new InvalidOperationException("Cannot execute command because account has not been signed in.");

                if (passAccount)
                    arguments += $" --account {QuoteArgument(selectedAccount)}";
                if (passSession)
                    arguments += $" --session {_session}";
                break;
        }

        if (_verbose)
            Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {command}");

        var startInfo = new ProcessStartInfo(_opPath, arguments)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = new UTF8Encoding(false),
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        startInfo.Environment["OP_BIOMETRIC_UNLOCK_ENABLED"] = _mode == Mode.AppIntegrated ? "true" : "false";
        if (_mode == Mode.ServiceAccount)
        {
            // Connect variables take precedence over an explicitly selected service account.
            startInfo.Environment.Remove("OP_CONNECT_HOST");
            startInfo.Environment.Remove("OP_CONNECT_TOKEN");
            startInfo.Environment["OP_SERVICE_ACCOUNT_TOKEN"] = _serviceAccountToken;
        }
        else
        {
            startInfo.Environment.Remove("OP_SERVICE_ACCOUNT_TOKEN");
        }

        if (secretKey is not null)
            startInfo.Environment["OP_SECRET_KEY"] = secretKey;

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException($"Could not start process for {_opPath}.");
        // Drain both pipes before writing input so either output stream can grow without blocking the CLI.
        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        foreach (var inputLine in input)
        {
            process.StandardInput.WriteLine(inputLine);
            process.StandardInput.Flush();
        }
        process.StandardInput.Close();

        process.WaitForExit();
        var output = outputTask.GetAwaiter().GetResult();
        var error = errorTask.GetAwaiter().GetResult();

        if (process.ExitCode == 0 && !error.StartsWith("[ERROR]", StringComparison.Ordinal))
            return output;

        if (allowDeviceRegistration && error.Contains("No saved device ID.", StringComparison.Ordinal))
            return error;

        var errorMessage = string.IsNullOrWhiteSpace(error) ? $"1Password CLI exited with code {process.ExitCode}." : error.Trim();
        throw new InvalidOperationException(errorMessage);
    }

    private static bool IsExcludedCommand(string command, IEnumerable<string> excludedCommands)
    {
        return excludedCommands.Any(x => string.Equals(command, x, StringComparison.Ordinal) || command.StartsWith(x + " ", StringComparison.Ordinal));
    }

    private static bool IsUnsupportedCommand(string command, IEnumerable<string> unsupportedCommands)
    {
        return unsupportedCommands.Any(x => string.Equals(command, x, StringComparison.Ordinal) || command.StartsWith(x + " ", StringComparison.Ordinal));
    }
#if NET7_0_OR_GREATER

    [GeneratedRegex(@"Version ([^\s]+) is now available\.", RegexOptions.Compiled)]
    private static partial Regex GeneratedVersionRegex();
#endif
}
