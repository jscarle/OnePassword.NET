using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using OnePassword.Common;

namespace OnePassword;

/// <summary>
/// Manages the 1Password CLI executable.
/// </summary>
public sealed partial class OnePasswordManager : IOnePasswordManager
{
    private static readonly Regex VersionRegex = new(@"Version ([^\s]+) is now available\.", RegexOptions.Compiled);
    private readonly string[] _excludedAccountCommands = { "--version", "update", "account list", "account add", "account forget", "signout --all" };
    private readonly string[] _excludedSessionCommands = { "--version", "update", "account list", "account add", "account forget", "signin", "signout", "signout --all" };
    private readonly Mode _mode = Mode.Interactive;
    private readonly string _opPath;
    private readonly string _serviceAccountToken;
    private readonly string[] _serviceAccountUnsupportedCommands = { "events-api", "group", "user" };
    private readonly bool _verbose;
    private string _account = "";
    private string _session = "";

    /// <summary>
    /// Initializes a new instance of <see cref="OnePasswordManager" /> using the specified options.
    /// </summary>
    /// <param name="options">The configuration options.</param>
    /// <exception cref="FileNotFoundException">Thrown when the 1Password CLI executable cannot be found.</exception>
    public OnePasswordManager(Action<OnePasswordManagerOptions> options)
        : this(ConfigureOptions(options))
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="OnePasswordManager" /> using the specified options.
    /// </summary>
    /// <param name="options">The configuration options.</param>
    /// <exception cref="FileNotFoundException">Thrown when the 1Password CLI executable cannot be found.</exception>
    public OnePasswordManager(OnePasswordManagerOptions? options = null)
    {
        var configuration = ValidateOptions(options);

        _opPath = configuration.Path.Length > 0 ? Path.Combine(configuration.Path, configuration.Executable) : Path.Combine(Directory.GetCurrentDirectory(), configuration.Executable);
        if (!File.Exists(_opPath))
            throw new FileNotFoundException($"The 1Password CLI executable ({configuration.Executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = configuration.Verbose;

        if (configuration.AppIntegrated)
            _mode = Mode.AppIntegrated;

        _serviceAccountToken = configuration.ServiceAccountToken;
        if (_serviceAccountToken.Length > 0)
            _mode = Mode.ServiceAccount;

        Version = GetVersion();
    }

    /// <summary>
    /// Initializes a new instance of <see cref="OnePasswordManager" /> for the specified 1Password CLI executable.
    /// </summary>
    /// <param name="path">The path to the 1Password CLI executable.</param>
    /// <param name="executable">The name of the 1Password CLI executable.</param>
    /// <param name="verbose">
    /// When <see langword="true" />, commands sent to the 1Password CLI executable are output to the
    /// console.
    /// </param>
    /// <param name="appIntegrated">
    /// Set to <see langword="true" /> when authentication is integrated into the 1Password desktop
    /// application (see <a href="https://developer.1password.com/docs/cli/get-started/#sign-in">documentation</a>). When
    /// <see langword="false" />, a password will be required to sign in.
    /// </param>
    /// <exception cref="FileNotFoundException">Thrown when the 1Password CLI executable cannot be found.</exception>
    [Obsolete($"This constructor is deprecated. Please use the constructor overload with '{nameof(OnePasswordManagerOptions)}' as argument.")]
    public OnePasswordManager(string path = "", string executable = "op.exe", bool verbose = false, bool appIntegrated = false)
    {
        _opPath = path.Length > 0 ? Path.Combine(path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
        if (!File.Exists(_opPath))
            throw new FileNotFoundException($"The 1Password CLI executable ({executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = verbose;

        if (appIntegrated)
            _mode = Mode.AppIntegrated;

        _serviceAccountToken = "";

        Version = GetVersion();
    }

    /// <inheritdoc />
    public string Version { get; private set; }

    /// <inheritdoc />
    public bool Update()
    {
        var updated = false;

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        var command = $"update --directory \"{tempDirectory}\"";
        var result = Op(command);

        var match = VersionRegex.Match(result);
        if (match.Success)
        {
            foreach (var file in Directory.GetFiles(tempDirectory, "*.zip"))
            {
                using var zipArchive = ZipFile.Open(file, ZipArchiveMode.Read);

                var entry = zipArchive.GetEntry("op.exe");
                if (entry is null)
                    continue;
                entry.ExtractToFile(_opPath, true);

                Version = GetVersion();
                updated = true;
            }
        }

        Directory.Delete(tempDirectory, true);

        return updated;
    }

    /// <inheritdoc />
    public string GetSecret(string reference)
    {
        var trimmedReference = reference.Trim();
        if (trimmedReference.Length == 0)
            throw new ArgumentException($"{nameof(trimmedReference)} cannot be empty.", nameof(reference));

        var command = $"read {reference} --no-newline";
        return Op(command);
    }

    /// <inheritdoc />
    public void SaveSecret(string reference, string filePath, string? fileMode = null)
    {
        var trimmedReference = reference.Trim();
        if (trimmedReference.Length == 0)
            throw new ArgumentException($"{nameof(trimmedReference)} cannot be empty.", nameof(reference));
        var trimmedFilePath = filePath.Trim();
        if (trimmedFilePath.Length == 0)
            throw new ArgumentException($"{nameof(trimmedFilePath)} cannot be empty.", nameof(filePath));

        var trimmedFileMode = fileMode?.Trim();
        var command = $"read {reference} --no-newline --force --out-file \"{trimmedFilePath}\"";
        if (trimmedFileMode is not null)
            command += $" --file-mode {trimmedFileMode}";
        Op(command);
    }

    private static OnePasswordManagerOptions ConfigureOptions(Action<OnePasswordManagerOptions> configure)
    {
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
        return Op(command);
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

    private TResult Op<TResult>(string command, string? input = null, bool returnError = false)
        where TResult : class
    {
        var result = Op(command, input is null ? Array.Empty<string>() : new[] { input }, returnError);
        var obj = JsonSerializer.Deserialize<TResult>(result) ?? throw new SerializationException("Could not deserialize the command result.");
        if (obj is ITracked item)
            item.AcceptChanges();
        return obj;
    }

    private string Op(string command, string? input = null, bool returnError = false)
    {
        return Op(command, input is null ? Array.Empty<string>() : new[] { input }, returnError);
    }

    private string Op(string command, IEnumerable<string> input, bool returnError)
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
                var passAccount = _account.Length != 0 && !excluded;
                if (requireAccount && !passAccount)
                    throw new InvalidOperationException("Cannot execute command because account has not been set.");

                var passSession = !(_mode == Mode.AppIntegrated || IsExcludedCommand(command, _excludedSessionCommands));
                if (passSession && _session.Length == 0)
                    throw new InvalidOperationException("Cannot execute command because account has not been signed in.");

                if (passAccount)
                    arguments += $" --account {_account}";
                if (passSession)
                    arguments += $" --session {_session}";
                break;
        }

        if (_verbose)
            Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

        var startInfo = new ProcessStartInfo(_opPath, arguments)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        if (_mode == Mode.ServiceAccount)
            startInfo.EnvironmentVariables["OP_SERVICE_ACCOUNT_TOKEN"] = _serviceAccountToken;

        var process = Process.Start(startInfo);

        if (process is null)
            throw new InvalidOperationException($"Could not start process for {_opPath}.");

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

    private static bool IsExcludedCommand(string command, IEnumerable<string> excludedCommands)
    {
        return excludedCommands.Any(x => command.StartsWith(x, StringComparison.InvariantCulture));
    }

    private static bool IsUnsupportedCommand(string command, IEnumerable<string> unsupportedCommands)
    {
        return unsupportedCommands.Any(x => command.StartsWith(x, StringComparison.InvariantCulture));
    }
}
