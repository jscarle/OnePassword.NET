using System.Globalization;
using System.IO.Compression;
using System.Runtime.InteropServices;
using OnePassword.Documents;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword.Common;

public class TestsBase
{
    private static readonly int CommandTimeout = GetIntEnv("OPT_COMMAND_TIMEOUT", 2) * 60 * 1000;
    private protected static readonly int RateLimit = GetIntEnv("OPT_RATE_LIMIT", 250);
    private protected static readonly bool RunLiveTests = GetBoolEnv("OPT_RUN_LIVE_TESTS", false);
    private protected static readonly bool CreateTestUser = GetBoolEnv("OPT_CREATE_TEST_USER", false);
    private protected static readonly string AccountAddress = GetEnv("OPT_ACCOUNT_ADDRESS", "");
    private protected static readonly string AccountEmail = GetEnv("OPT_ACCOUNT_EMAIL", "");
    private protected static readonly string AccountName = GetEnv("OPT_ACCOUNT_NAME", "");
    private protected static readonly string AccountPassword = GetEnv("OPT_ACCOUNT_PASSWORD", "");
    private protected static readonly string AccountSecretKey = GetEnv("OPT_ACCOUNT_SECRET_KEY", "");
    private protected static readonly string TestUserEmail = GetEnv("OPT_TEST_USER_EMAIL", "");
    private static readonly string ServiceAccountToken = GetEnv("OPT_SERVICE_ACCOUNT_TOKEN", "");
    private protected static readonly int TestUserConfirmTimeout = GetIntEnv("OPT_TEST_USER_CONFIRM_TIMEOUT", GetIntEnv("OPT_COMMAND_TIMEOUT", 2)) * 60 * 1000;
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    private static readonly CancellationTokenSource TestCancellationTokenSource = new();
    private static readonly CancellationTokenSource TearDownCancellationTokenSource = new();
    private static readonly Uri DownloadSource = GetDownloadSource();
    private static readonly string ExecutableName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "op.exe" : "op";
    private static bool _initialSetupDone;

    private protected static readonly CancellationTokenSource SetUpCancellationTokenSource = new();
    private protected static OnePasswordManager OnePassword = null!;
    private protected static IUser TestUser = null!;
    private protected static IGroup TestGroup = null!;
    private protected static IVault TestVault = null!;
    private protected static IDocument TestDocument = null!;
    private protected static Template TestTemplate = null!;
    private protected static Item TestItem = null!;
    private protected static bool GroupManagementSupported = true;
    private protected static bool UserManagementSupported = true;
    private protected static bool DoFinalTearDown;
    private protected static readonly string WorkingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

    [OneTimeSetUp]
    public async Task Setup()
    {
        if (!RunLiveTests || _initialSetupDone)
            return;

        Directory.CreateDirectory(WorkingDirectory);

        var zipFileName = Path.Combine(WorkingDirectory, Path.GetFileName(DownloadSource.LocalPath));
        var extractFilename = Path.Combine(WorkingDirectory, ExecutableName);

        using var httpClient = new HttpClient();
        await using (var stream = await httpClient.GetStreamAsync(DownloadSource.OriginalString))
        await using (var fileStream = new FileStream(zipFileName, FileMode.Create))
            await stream.CopyToAsync(fileStream);

        using var zipArchive = ZipFile.Open(zipFileName, ZipArchiveMode.Read);
        var entry = zipArchive.GetEntry(ExecutableName) ?? throw new IOException($"Could not find {ExecutableName} in the zip file.");
        entry.ExtractToFile(extractFilename, true);

        OnePassword = new OnePasswordManager(options =>
        {
            options.Path = WorkingDirectory;
            options.Executable = ExecutableName;
            options.ServiceAccountToken = ServiceAccountToken;
        });

        _initialSetupDone = true;
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        if (!RunLiveTests || !DoFinalTearDown)
            return;

        Directory.Delete(WorkingDirectory, true);
    }

    protected static void Run(RunType runType, Action action)
    {
        var tokenSource = runType switch
        {
            RunType.SetUp => SetUpCancellationTokenSource,
            RunType.Test => TestCancellationTokenSource,
            RunType.TearDown => TearDownCancellationTokenSource,
            _ => throw new NotImplementedException()
        };

        SemaphoreSlim.Wait(CommandTimeout, tokenSource.Token);
        try
        {
            action();
        }
        catch (NUnit.Framework.IgnoreException)
        {
            throw;
        }
        catch (Exception)
        {
            tokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    private static bool GetBoolEnv(string name, bool value)
    {
        var environmentValue = GetEnv(name, value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
        return bool.TryParse(environmentValue, out var parsedValue) ? parsedValue : value;
    }

    private static int GetIntEnv(string name, int value)
    {
        var environmentValue = GetEnv(name, value.ToString(CultureInfo.InvariantCulture));
        return int.TryParse(environmentValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue) ? parsedValue : value;
    }

    private static string GetEnv(string name, string value)
    {
        foreach (var target in new[]
                 {
                     EnvironmentVariableTarget.Machine,
                     EnvironmentVariableTarget.User,
                     EnvironmentVariableTarget.Process
                 })
        {
            var environmentValue = Environment.GetEnvironmentVariable(name, target);
            if (!string.IsNullOrWhiteSpace(environmentValue))
                return environmentValue;
        }

        return value;
    }

    private static Uri GetDownloadSource()
    {
        const string version = "v2.26.0";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new Uri($"https://cache.agilebits.com/dist/1P/op2/pkg/{version}/op_windows_amd64_{version}.zip");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new Uri($"https://cache.agilebits.com/dist/1P/op2/pkg/{version}/op_darwin_amd64_{version}.zip");

        return new Uri($"https://cache.agilebits.com/dist/1P/op2/pkg/{version}/op_linux_amd64_{version}.zip");
    }

    private protected static void MarkManagementUnsupported()
    {
        GroupManagementSupported = false;
        UserManagementSupported = false;
    }

    protected enum RunType
    {
        SetUp,
        Test,
        TearDown
    }
}
