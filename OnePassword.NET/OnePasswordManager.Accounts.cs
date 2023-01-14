using System.Text.RegularExpressions;
using OnePassword.Accounts;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly Regex ForgottenAccountsRegex = new(@"""([^""]+)""", RegexOptions.Compiled);
    private static readonly Regex DeviceRegex = new("OP_DEVICE=(?<UUID>[a-z0-9]+)", RegexOptions.Compiled);

    /// <summary>
    /// Gets the accounts.
    /// </summary>
    /// <returns>The list of accounts.</returns>
    public ImmutableList<Account> GetAccounts()
    {
        const string command = "account list";
        return Op<ImmutableList<Account>>(command);
    }

    /// <summary>
    /// Gets the account details.
    /// </summary>
    /// <param name="account">The account to retrieve.</param>
    /// <returns>The account details.</returns>
    public AccountDetails GetAccount(string account = "")
    {
        var trimmedAccount = account.Trim();

        var command = trimmedAccount.Length > 0 ? $"account get --account \"{trimmedAccount}\"" : "account get";
        return Op<AccountDetails>(command);
    }
    
    /// <summary>
    /// Adds an account.
    /// </summary>
    /// <param name="address">The account address.</param>
    /// <param name="email">The account email.</param>
    /// <param name="secretKey">The account secret key.</param>
    /// <param name="password">The account password.</param>
    /// <param name="shorthand">The account shorthand.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "")
    {
        var trimmedAddress = address.Trim();
        if (trimmedAddress.Length == 0)
            throw new ArgumentException($"{nameof(address)} cannot be empty.", nameof(address));

        var trimmedEmail = email.Trim();
        if (trimmedEmail.Length == 0)
            throw new ArgumentException($"{nameof(email)} cannot be empty.", nameof(email));

        var trimmedSecretKey = secretKey.Trim();
        if (trimmedSecretKey.Length == 0)
            throw new ArgumentException($"{nameof(secretKey)} cannot be empty.", nameof(secretKey));

        if (password.Length == 0)
            throw new ArgumentException($"{nameof(password)} cannot be empty.", nameof(password));

        var trimmedShorthand = shorthand.Trim();

        var command = $"account add --address \"{trimmedAddress}\" --email \"{trimmedEmail}\" --secret-key \"{trimmedSecretKey}\"";
        if (trimmedShorthand.Length > 0)
            command += $" --shorthand \"{trimmedShorthand}\"";

        var result = Op(command, password, true);
        if (result.Contains("No saved device ID."))
        {
            var deviceUuid = DeviceRegex.Match(result).Groups["UUID"].Value;
            Environment.SetEnvironmentVariable("OP_DEVICE", deviceUuid);
            Op(command, password);
        }

        _account = trimmedShorthand.Length > 0 ? trimmedShorthand : trimmedAddress;
    }

    /// <summary>
    /// Uses the account.
    /// </summary>
    /// <param name="account">The account to use.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void UseAccount(string account)
    {
        var trimmedAccount = account.Trim();
        if (trimmedAccount.Length == 0)
            throw new ArgumentException($"{nameof(account)} cannot be empty.", nameof(account));

        _account = trimmedAccount;
    }

    /// <summary>
    /// Signs in to the account.
    /// </summary>
    /// <param name="password">The account password.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SignIn(string password)
    {
        if (password.Length == 0)
            throw new ArgumentException($"{nameof(password)} cannot be empty.", nameof(password));

        const string command = "signin --force --raw";
        var result = Op(command, password);
        _session = result.Trim();
    }

    /// <summary>
    /// Signs into the account using 1Password app integration.
    /// </summary>
    public void SignIn()
    {
        const string command = "signin --force";
        _ = Op(command);
    }

    /// <summary>
    /// Signs out of the account.
    /// </summary>
    /// <param name="all">When <see langword="true"/>, signs out of all accounts.</param>
    public void SignOut(bool all = false)
    {
        var command = "signout";
        if (all)
            command += " --all";
        Op(command);
    }

    /// <summary>
    /// Forgets the account.
    /// </summary>
    /// <param name="all">When <see langword="true"/>, forgets all accounts.</param>
    /// <returns>The list of accounts that were forgotten.</returns>
    public ImmutableList<string> ForgetAccount(bool all = false)
    {
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