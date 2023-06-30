using System.Text.RegularExpressions;
using OnePassword.Accounts;
using OnePassword.Common;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly Regex ForgottenAccountsRegex = new(@"""([^""]+)""", RegexOptions.Compiled);
    private static readonly Regex DeviceRegex = new("OP_DEVICE=(?<UUID>[a-z0-9]+)", RegexOptions.Compiled);

    /// <inheritdoc />
    public ImmutableList<Account> GetAccounts()
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(GetAccounts)} is not supported when using service accounts.");

        const string command = "account list";
        return Op<ImmutableList<Account>>(command);
    }

    /// <inheritdoc />
    public AccountDetails GetAccount(string account = "")
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(GetAccount)} is not supported when using service accounts.");

        var trimmedAccount = account.Trim();

        var command = trimmedAccount.Length > 0 ? $"account get --account \"{trimmedAccount}\"" : "account get";
        return Op<AccountDetails>(command);
    }

    /// <inheritdoc />
    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "")
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(AddAccount)} is not supported when using service accounts.");
        
        var trimmedAddress = address.Trim();
        if (trimmedAddress.Length == 0)
            throw new ArgumentException($"{nameof(address)} cannot be empty.", nameof(address));

        var trimmedEmail = email.Trim();
        if (trimmedEmail.Length == 0)
            throw new ArgumentException($"{nameof(email)} cannot be empty.", nameof(email));

        var trimmedSecretKey = secretKey.Trim();
        if (trimmedSecretKey.Length == 0)
            throw new ArgumentException($"{nameof(secretKey)} cannot be empty.", nameof(secretKey));

        var trimmedPassword = password.Trim();
        if (trimmedPassword.Length == 0)
            throw new ArgumentException($"{nameof(password)} cannot be empty.", nameof(password));

        var trimmedShorthand = shorthand.Trim();

        var command = $"account add --address \"{trimmedAddress}\" --email \"{trimmedEmail}\" --secret-key \"{trimmedSecretKey}\"";
        if (trimmedShorthand.Length > 0)
            command += $" --shorthand \"{trimmedShorthand}\"";

        var result = Op(command, trimmedPassword, true);
        if (result.Contains("No saved device ID."))
        {
            var deviceUuid = DeviceRegex.Match(result).Groups["UUID"].Value;
            Environment.SetEnvironmentVariable("OP_DEVICE", deviceUuid);
            Op(command, password);
        }

        _account = trimmedShorthand.Length > 0 ? trimmedShorthand : trimmedAddress;
    }

    /// <inheritdoc />
    public void UseAccount(string account)
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(UseAccount)} is not supported when using service accounts.");

        var trimmedAccount = account.Trim();
        if (trimmedAccount.Length == 0)
            throw new ArgumentException($"{nameof(account)} cannot be empty.", nameof(account));

        _account = trimmedAccount;
    }

    /// <inheritdoc />
    public void SignIn(string? password = null)
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(SignIn)} is not supported when using service accounts.");

        var trimmedPassword = password?.Trim();
        switch (_mode == Mode.AppIntegrated)
        {
            case true when trimmedPassword is not null:
                throw new ArgumentException($"{nameof(password)} cannot be supplied when authentication is integrated into the 1Password desktop application.", nameof(password));
            case false when trimmedPassword is null || trimmedPassword.Length == 0:
                throw new ArgumentException($"{nameof(password)} cannot be empty.", nameof(password));
        }

        const string command = "signin --force --raw";
        var result = Op(command, password?.Trim());
        _session = result.Trim();
    }

    /// <inheritdoc />
    public void SignOut(bool all = false)
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(SignOut)} is not supported when using service accounts.");

        var command = "signout";
        if (all)
            command += " --all";
        Op(command);
    }

    /// <inheritdoc />
    public ImmutableList<string> ForgetAccount(bool all = false)
    {
        if (_mode == Mode.ServiceAccount)
            throw new InvalidOperationException($"{nameof(ForgetAccount)} is not supported when using service accounts.");

        var accounts = ImmutableList.CreateBuilder<string>();

        var command = "account forget";
        command += all ? " --all" : $" \"{_account}\"";

        var result = Op(command);

        if (all)
            foreach (var match in ForgottenAccountsRegex.Matches(result).Cast<Match>())
                accounts.Add(match.Groups[1].Value);
        else
            accounts.Add(_account);

        return accounts.ToImmutable();
    }
}