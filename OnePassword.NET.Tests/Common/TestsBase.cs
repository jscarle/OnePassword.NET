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
    private static readonly int CommandTimeout = int.Parse(GetEnv("OPT_COMMAND_TIMEOUT", "2"), CultureInfo.InvariantCulture) * 60 * 1000;
    private protected static readonly int RateLimit = int.Parse(GetEnv("OPT_RATE_LIMIT", "250"), CultureInfo.InvariantCulture);
    private protected static readonly bool RunLiveTests = bool.Parse(GetEnv("OPT_RUN_LIVE_TESTS", "false"));
    private protected static readonly bool CreateTestUser = bool.Parse(GetEnv("OPT_CREATE_TEST_USER", "false"));
    private protected static readonly string AccountAddress = GetEnv("OPT_ACCOUNT_ADDRESS", "");
    private protected static readonly string AccountEmail = GetEnv("OPT_ACCOUNT_EMAIL", "");
    private protected static readonly string AccountName = GetEnv("OPT_ACCOUNT_NAME", "");
    private protected static readonly string AccountPassword = GetEnv("OPT_ACCOUNT_PASSWORD", "");
    private protected static readonly string AccountSecretKey = GetEnv("OPT_ACCOUNT_SECRET_KEY", "");
    private protected static readonly string TestUserEmail = GetEnv("OPT_TEST_USER_EMAIL", "");
    private static readonly string ServiceAccountToken = GetEnv("OPT_SERVICE_ACCOUNT_TOKEN", "");
    private protected static readonly int TestUserConfirmTimeout = int.Parse(GetEnv("OPT_TEST_USER_CONFIRM_TIMEOUT", GetEnv("OPT_COMMAND_TIMEOUT", "2")), CultureInfo.InvariantCulture) * 60 * 1000;
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    private protected static readonly CancellationTokenSource SetUpCancellationTokenSource = new();
    private static readonly CancellationTokenSource TestCancellationTokenSource = new();
    private static readonly CancellationTokenSource TearDownCancellationTokenSource = new();
    private protected static OnePasswordManager OnePassword = null!;
    private protected static IUser TestUser = null!;
    private protected static IGroup TestGroup = null!;
    private protected static IVault TestVault = null!;
    private protected static IDocument TestDocument = null!;
    private protected static Template TestTemplate = null!;
    private protected static Item TestItem = null!;
    private protected static bool DoFinalTearDown;
    private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    private static readonly Uri DownloadSource = IsLinux ?
        new Uri("https://cache.agilebits.com/dist/1P/op2/pkg/v2.18.0/op_linux_amd64_v2.18.0.zip") :
        new Uri("https://cache.agilebits.com/dist/1P/op2/pkg/v2.18.0/op_windows_amd64_v2.18.0.zip");
    private static readonly string ExecutableName = IsLinux ? "op" : "op.exe";
    private protected static readonly string WorkingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    private static bool _initialSetupDone;

    private static string GetEnv(string name, string value) =>
        Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine)
        ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User)
        ?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)
        ?? value;

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
        var entry = zipArchive.GetEntry(ExecutableName);
        if (entry is null)
            throw new IOException($"Could not find {ExecutableName} in the zip file.");
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

    protected enum RunType
    {
        SetUp,
        Test,
        TearDown
    }
}