using System.IO.Compression;
using System.Runtime.InteropServices;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword.Common;

public class TestsBase
{
    private protected static readonly bool RunLiveTests = bool.Parse(GetEnv("OPT_RUN_LIVE_TESTS", "false"));
    private protected static readonly bool CreateTestUser = bool.Parse(GetEnv("OPT_CREATE_TEST_USER", "false"));
    private protected static readonly string AccountAddress = GetEnv("OPT_ACCOUNT_ADDRESS", "");
    private protected static readonly string AccountEmail = GetEnv("OPT_ACCOUNT_EMAIL", "");
    private protected static readonly string AccountName = GetEnv("OPT_ACCOUNT_NAME", "");
    private protected static readonly string AccountPassword = GetEnv("OPT_ACCOUNT_PASSWORD", "");
    private protected static readonly string AccountSecretKey = GetEnv("OPT_ACCOUNT_SECRET_KEY", "");
    private protected static readonly string TestUserEmail = GetEnv("OPT_TEST_USER_EMAIL", "");
    private protected const int CommandTimeout = 2 * 60 * 1000;
    private protected const int RateLimit = 250;
    private protected static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    private protected static readonly CancellationTokenSource SetUpCancellationTokenSource = new();
    private protected static readonly CancellationTokenSource TearDownCancellationTokenSource = new();
    private protected static OnePasswordManager OnePassword = null!;
    private protected static IUser TestUser = null!;
    private protected static IGroup TestGroup = null!;
    private protected static IVault TestVault = null!;
    private protected static Template TestTemplate = null!;
    private protected static Item TestItem = null!;
    private protected static bool DoFinalTearDown = false;
    private static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    private static readonly Uri DownloadSource = IsLinux ?
        new Uri("https://cache.agilebits.com/dist/1P/op2/pkg/v2.11.0/op_linux_amd64_v2.11.0.zip") :
        new Uri("https://cache.agilebits.com/dist/1P/op2/pkg/v2.11.0/op_windows_amd64_v2.11.0.zip");
    private static readonly string ExecutableName = IsLinux ? "op" : "op.exe";
    private static readonly string WorkingDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    private static bool _initialSetupDone;

    private static string GetEnv(string name, string value)
    {
        return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine) ??
               Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User) ??
               Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process) ??
               value;
    }

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
            throw new Exception($"Could not find {ExecutableName} in the zip file.");
        entry.ExtractToFile(extractFilename, true);

        OnePassword = new OnePasswordManager(WorkingDirectory, ExecutableName);

        _initialSetupDone = true;
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        if (!RunLiveTests || !DoFinalTearDown)
            return;

        Directory.Delete(WorkingDirectory, true);
    }
}