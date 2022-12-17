using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using OnePassword.Common;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public string Version { get; private set; }

    private readonly string[] _excludedAccountCommands = { "--version", "update", "account list", "account add", "account forget", "signout --all" };
    private readonly string[] _excludedSessionCommands = { "--version", "update", "account list", "account add", "account forget", "signin", "signout --all" };

    private readonly string _opPath;
    private readonly bool _verbose;
    private string _account = "";
    private string _session = "";

    public OnePasswordManager(string path = "", string executable = "op.exe", bool verbose = false)
    {
        _opPath = path.Length > 0 ? Path.Combine(path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
        if (!File.Exists(_opPath))
            throw new ArgumentException($"The 1Password CLI executable ({executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = verbose;

        Version = GetVersion();
    }

    public bool Update()
    {
        var updated = false;

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        var command = $"update --directory \"{tempDirectory}\"";
        var result = Op(command);

        var match = Regex.Match(result, @"Version ([^\s]+) is now available\.");
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
        var passAccount = !IsExcludedCommand(command, _excludedAccountCommands);
        if (passAccount && _account.Length == 0)
            throw new InvalidOperationException("Cannot execute command because account has not been set.");

        var passSession = !IsExcludedCommand(command, _excludedSessionCommands);
        if (passSession && _session.Length == 0)
            throw new InvalidOperationException("Cannot execute command because account has not been signed in.");

        var arguments = command;
        if (command != "--version")
            arguments += " --format json --no-color";
        if (passAccount)
            arguments += $" --account {_account}";
        if (passSession)
            arguments += $" --session {_session}";

        if (_verbose)
            Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

        var process = Process.Start(new ProcessStartInfo(_opPath, arguments)
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        });

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
}