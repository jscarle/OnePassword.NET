using System.Globalization;
using System.Runtime.InteropServices;
using OnePassword.Common;
using OnePassword.Documents;
using OnePassword.Items;
using OnePassword.Vaults;

namespace OnePassword;

[TestFixture]
public class OnePasswordManagerCommandTests
{
    private static readonly string[] ParsedRecipients = ["one@example.com", "two@example.com"];

    [Test]
    public void VersionIsTrimmed()
    {
        using var fakeCli = new FakeCli(versionOutput: "2.32.1\r\n");

        var manager = fakeCli.CreateManager();

        Assert.That(manager.Version, Is.EqualTo("2.32.1"));
    }

    [Test]
    public void ArchiveDocumentObjectOverloadUsesArchiveCommand()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ArchiveDocument(new TestDocument("document-id"), new TestVault("vault-id"));

        Assert.That(fakeCli.LastArguments, Does.StartWith("document delete document-id --vault vault-id --archive"));
    }

    [Test]
    public void ArchiveDocumentStringOverloadUsesArchiveCommand()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ArchiveDocument("document-id", "vault-id");

        Assert.That(fakeCli.LastArguments, Does.StartWith("document delete document-id --vault vault-id --archive"));
    }

    [Test]
    public void ArchiveItemObjectOverloadUsesArchiveCommand()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ArchiveItem(new TestItem("item-id"), new TestVault("vault-id"));

        Assert.That(fakeCli.LastArguments, Does.StartWith("item delete item-id --vault vault-id --archive"));
    }

    [Test]
    public void ArchiveItemStringOverloadUsesArchiveCommand()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ArchiveItem("item-id", "vault-id");

        Assert.That(fakeCli.LastArguments, Does.StartWith("item delete item-id --vault vault-id --archive"));
    }

    [Test]
    public void ShareItemWithoutEmailsOmitsEmailsFlag()
    {
        using var fakeCli = new FakeCli(nextOutput: "https://share.example/item\r\n");
        var manager = fakeCli.CreateManager();

        var result = manager.ShareItem("item-id", "vault-id");

        Assert.Multiple(() =>
        {
            Assert.That(result.Url, Is.EqualTo(new Uri("https://share.example/item")));
            Assert.That(result.ExpiresAt, Is.Null);
            Assert.That(result.Recipients, Is.Empty);
            Assert.That(result.ViewOnce, Is.Null);
            Assert.That(fakeCli.LastArguments, Does.StartWith("item share item-id --vault vault-id"));
            Assert.That(fakeCli.LastArguments, Does.Not.Contain("--emails"));
        });
    }

    [Test]
    public void ShareItemStringSingleEmailOverloadUsesEmailsFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        var result = manager.ShareItem("item-id", "vault-id", "recipient@example.com");

        Assert.Multiple(() =>
        {
            Assert.That(result.Url, Is.Null);
            Assert.That(result.Recipients, Is.Empty);
            Assert.That(fakeCli.LastArguments, Does.Contain("--emails recipient@example.com"));
        });
    }

    [Test]
    public void ShareItemObjectSingleEmailOverloadUsesEmailsFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem(new TestItem("item-id"), new TestVault("vault-id"), "recipient@example.com");

        Assert.That(fakeCli.LastArguments, Does.Contain("--emails recipient@example.com"));
    }

    [Test]
    public void ShareItemWithSingleEmailCollectionUsesEmailsFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem("item-id", "vault-id", ["recipient@example.com"]);

        Assert.That(fakeCli.LastArguments, Does.Contain("--emails recipient@example.com"));
    }

    [Test]
    public void ShareItemWithMultipleEmailsUsesCommaSeparatedEmails()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem("item-id", "vault-id", ["one@example.com", "two@example.com"]);

        Assert.That(fakeCli.LastArguments, Does.Contain("--emails one@example.com,two@example.com"));
    }

    [Test]
    public void ShareItemWithEmptyEmailCollectionOmitsEmailsFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem("item-id", "vault-id", Array.Empty<string>());

        Assert.That(fakeCli.LastArguments, Does.Not.Contain("--emails"));
    }

    [Test]
    public void ShareItemWithExpiresInUsesExpiresInFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem("item-id", "vault-id", expiresIn: TimeSpan.FromDays(7));

        Assert.That(fakeCli.LastArguments, Does.Contain("--expires-in 7d"));
    }

    [Test]
    public void ShareItemWithViewOnceUsesViewOnceFlag()
    {
        using var fakeCli = new FakeCli();
        var manager = fakeCli.CreateManager();

        manager.ShareItem("item-id", "vault-id", viewOnce: true);

        Assert.That(fakeCli.LastArguments, Does.Contain("--view-once"));
    }

    [Test]
    public void ShareItemParsesStructuredShareResult()
    {
        const string response = """
                                {
                                  "share_link": "https://share.example/item",
                                  "expires_at": "2026-03-15T12:00:00Z",
                                  "view_once": true,
                                  "recipients": [
                                    { "email": "one@example.com" },
                                    { "address": "two@example.com" }
                                  ]
                                }
                                """;

        using var fakeCli = new FakeCli(nextOutput: response);
        var manager = fakeCli.CreateManager();

        var result = manager.ShareItem("item-id", "vault-id");

        Assert.Multiple(() =>
        {
            Assert.That(result.Url, Is.EqualTo(new Uri("https://share.example/item")));
            Assert.That(result.ExpiresAt, Is.EqualTo(DateTimeOffset.Parse("2026-03-15T12:00:00Z", CultureInfo.InvariantCulture)));
            Assert.That(result.ViewOnce, Is.True);
            Assert.That(result.Recipients, Is.EqualTo(ParsedRecipients));
        });
    }

    private sealed class FakeCli : IDisposable
    {
        private readonly string _argumentsPath;
        private readonly string _directoryPath;
        private readonly string _nextOutputPath;
        private readonly string _versionOutputPath;

        public FakeCli(string versionOutput = "2.32.1\n", string nextOutput = "{}")
        {
            _directoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            _argumentsPath = Path.Combine(_directoryPath, "last-arguments.txt");
            _nextOutputPath = Path.Combine(_directoryPath, "next-output.txt");
            _versionOutputPath = Path.Combine(_directoryPath, "version-output.txt");

            Directory.CreateDirectory(_directoryPath);
            File.WriteAllText(_nextOutputPath, nextOutput);
            File.WriteAllText(_versionOutputPath, versionOutput);

            var executablePath = Path.Combine(_directoryPath, ExecutableName);
            File.WriteAllText(executablePath, GetScript());
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                File.SetUnixFileMode(executablePath,
                    UnixFileMode.UserRead
                    | UnixFileMode.UserWrite
                    | UnixFileMode.UserExecute);
            }
        }

        public string ExecutableName { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "op.cmd" : "op";

        public string LastArguments => File.Exists(_argumentsPath) ? File.ReadAllText(_argumentsPath) : "";

        public OnePasswordManager CreateManager()
        {
            return new OnePasswordManager(options =>
            {
                options.Path = _directoryPath;
                options.Executable = ExecutableName;
                options.ServiceAccountToken = "test-token";
            });
        }

        public void Dispose()
        {
            if (Directory.Exists(_directoryPath))
                Directory.Delete(_directoryPath, true);
        }

        private static string GetScript()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? """
                  @echo off
                  setlocal
                  > "%~dp0last-arguments.txt" echo %*
                  if "%~1"=="--version" (
                    type "%~dp0version-output.txt"
                    exit /b 0
                  )
                  type "%~dp0next-output.txt"
                  """
                : """
                  #!/bin/sh
                  script_dir=$(CDPATH= cd -- "$(dirname "$0")" && pwd)
                  printf '%s' "$*" > "$script_dir/last-arguments.txt"
                  if [ "$1" = "--version" ]; then
                    cat "$script_dir/version-output.txt"
                    exit 0
                  fi
                  cat "$script_dir/next-output.txt"
                  """;
        }
    }

    private sealed class TestDocument(string id) : IDocument
    {
        public string Id { get; } = id;
    }

    private sealed class TestItem(string id) : IItem
    {
        public string Id { get; } = id;
    }

    private sealed class TestVault(string id, string name = "Test Vault") : IVault
    {
        public string Id { get; } = id;

        public string Name { get; } = name;

        public void Deconstruct(out string vaultId, out string vaultName)
        {
            vaultId = Id;
            vaultName = Name;
        }

        public bool Equals(IResult<IVault>? other) => other is not null && string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj) => obj is IResult<IVault> other && Equals(other);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Id);

        public int CompareTo(IResult<IVault>? other) => other is null ? 1 : string.Compare(Id, other.Id, StringComparison.Ordinal);

        public int CompareTo(object? obj)
        {
            if (obj is null)
                return 1;
            if (obj is IResult<IVault> other)
                return CompareTo(other);
            throw new ArgumentException($"Object must be of type {nameof(IResult<IVault>)}.", nameof(obj));
        }
    }
}
