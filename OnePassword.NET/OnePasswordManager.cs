using System.Collections;
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
    private static readonly string[][] ExcludedAccountCommands =
    [
        ["--version"],
        ["update"],
        ["account", "list"],
        ["account", "add"],
        ["account", "forget"],
        ["signout", "--all"]
    ];

    private static readonly string[][] ExcludedSessionCommands =
    [
        ["--version"],
        ["update"],
        ["account", "list"],
        ["account", "add"],
        ["account", "forget"],
        ["signin"],
        ["signout"],
        ["signout", "--all"]
    ];

    private static readonly string[][] ServiceAccountUnsupportedCommands =
    [
        ["events-api"],
        ["group"],
        ["user"]
    ];

    private readonly Mode _mode = Mode.Interactive;
    private readonly string _opPath;
    private readonly string _serviceAccountToken;
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

        var command = new OpCommand("update", "--directory", tempDirectory);
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

        Directory.Delete(tempDirectory, true);

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

        var command = new OpCommand("read", trimmedReference, "--no-newline");
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
        var command = new OpCommand("read", trimmedReference, "--no-newline", "--force", "--out-file", trimmedFilePath);
        if (trimmedFileMode is not null && trimmedFileMode.Length > 0)
            command.Add("--file-mode", trimmedFileMode);
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
        var command = new OpCommand("--version");
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

    private static void ApplyFileMode(string fileMode, string filePath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return;

        using var chmod = Process.Start(new ProcessStartInfo("chmod", $"{fileMode} \"{filePath}\"")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });
        chmod?.WaitForExit();
    }

    private static string GetStandardError(Process process)
    {
        var error = new StringBuilder();
        while (process.StandardError.Peek() > -1)
            error.Append((char)process.StandardError.Read());
        return error.ToString();
    }

    private static string GetStandardOutput(Process process)
    {
        var output = new StringBuilder();
        while (process.StandardOutput.Peek() > -1)
            output.Append((char)process.StandardOutput.Read());
        return output.ToString();
    }

    private TResult Op<TResult>(JsonTypeInfo<TResult> jsonTypeInfo, OpCommand command, string? input = null, bool returnError = false) where TResult : class
    {
        var result = Op(command, input is null ? Array.Empty<string>() : [input], returnError);
        var obj = JsonSerializer.Deserialize(result, jsonTypeInfo) ?? throw new SerializationException("Could not deserialize the command result.");
        if (obj is ITracked item)
            item.AcceptChanges();
        return obj;
    }

    private string Op(OpCommand command, string? input = null, bool returnError = false, bool formatOutput = true) => Op(command, input is null ? Array.Empty<string>() : [input], returnError, formatOutput);

    private string Op(OpCommand command, IEnumerable<string> input, bool returnError, bool formatOutput = true)
    {
        var arguments = command.Clone();
        if (!command.StartsWith(["--version"]) && formatOutput)
            arguments.Add("--format", "json", "--no-color");

        switch (_mode)
        {
            case Mode.ServiceAccount:
                if (IsCommandMatch(command, ServiceAccountUnsupportedCommands))
                    throw new InvalidOperationException($"Unsupported command {command} when using ServiceAccount");
                break;
            case Mode.Interactive:
            case Mode.AppIntegrated:
            default:
                var excluded = IsCommandMatch(command, ExcludedAccountCommands);
                var requireAccount = _mode != Mode.AppIntegrated && !excluded;
                var passAccount = _account.Length != 0 && !excluded;
                if (requireAccount && !passAccount)
                    throw new InvalidOperationException("Cannot execute command because account has not been set.");

                var passSession = !(_mode == Mode.AppIntegrated || IsCommandMatch(command, ExcludedSessionCommands));
                if (passSession && _session.Length == 0)
                    throw new InvalidOperationException("Cannot execute command because account has not been signed in.");

                if (passAccount)
                    arguments.Add("--account", _account);
                if (passSession)
                    arguments.Add("--session", _session);
                break;
        }

        if (_verbose)
            Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

        var startInfo = new ProcessStartInfo(_opPath)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };
        if (IsWindowsCommandScript(_opPath))
        {
            startInfo.Arguments = arguments.ToString();
        }
        else
        {
            foreach (var argument in arguments)
                startInfo.ArgumentList.Add(argument);
        }

        if (_mode == Mode.ServiceAccount)
            startInfo.EnvironmentVariables["OP_SERVICE_ACCOUNT_TOKEN"] = _serviceAccountToken;

        var process = Process.Start(startInfo) ?? throw new InvalidOperationException($"Could not start process for {_opPath}.");
        foreach (var inputLine in input)
        {
            var lastChar = inputLine.Substring(inputLine.Length - 1, 1);
            if (lastChar == "\x04")
            {
                process.StandardInput.WriteLine(inputLine[..^1]);
                process.StandardInput.Flush();
            }
            else
            {
                process.StandardInput.WriteLine(inputLine);
                process.StandardInput.Flush();
            }
        }
        process.StandardInput.Close();

        var output = GetStandardOutput(process);
        if (_verbose)
            Console.WriteLine(output);

        var error = GetStandardError(process);
        if (_verbose)
            Console.WriteLine(error);

        if (!error.StartsWith("[ERROR]", StringComparison.InvariantCulture))
            return output;

        if (returnError)
            return error;

        throw new InvalidOperationException(error.Length > 28 ? error[28..].Trim() : error);
    }

    private static bool IsCommandMatch(OpCommand command, IEnumerable<IReadOnlyList<string>> commandPrefixes)
    {
        return commandPrefixes.Any(command.StartsWith);
    }

    private static bool IsWindowsCommandScript(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
               && (string.Equals(extension, ".cmd", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(extension, ".bat", StringComparison.OrdinalIgnoreCase));
    }

    private sealed class OpCommand : IEnumerable<string>
    {
        private readonly List<string> _arguments;

        public OpCommand(params string[] arguments)
        {
            _arguments = [.. arguments];
        }

        private OpCommand(IEnumerable<string> arguments)
        {
            _arguments = [.. arguments];
        }

        public OpCommand Add(params string[] arguments)
        {
            _arguments.AddRange(arguments);
            return this;
        }

        public OpCommand Clone() => new(_arguments);

        public IEnumerator<string> GetEnumerator() => _arguments.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool StartsWith(IReadOnlyList<string> prefix)
        {
            return prefix.Count <= _arguments.Count
                   && prefix.Select((argument, index) => string.Equals(argument, _arguments[index], StringComparison.Ordinal)).All(static matches => matches);
        }

        public override string ToString() => string.Join(" ", _arguments.Select(FormatArgument));

        private static string FormatArgument(string argument)
        {
            return argument.Any(char.IsWhiteSpace) || argument.Contains('"', StringComparison.Ordinal)
                ? $"\"{argument.Replace("\"", "\\\"", StringComparison.Ordinal)}\""
                : argument;
        }
    }
#if NET7_0_OR_GREATER

    [GeneratedRegex(@"Version ([^\s]+) is now available\.", RegexOptions.Compiled)]
    private static partial Regex GeneratedVersionRegex();
#endif
}
