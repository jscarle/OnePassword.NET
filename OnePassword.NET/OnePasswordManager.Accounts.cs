using System.Collections.Immutable;
using System.Text.RegularExpressions;
using OnePassword.Accounts;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly Regex ForgottenAccountsRegex = new(@"""([^""]+)""", RegexOptions.Compiled);

    public ImmutableList<Account> GetAccounts()
    {
        var command = "account list";
        return Op<ImmutableList<Account>>(command, false, false);
    }

    public AccountDetails GetAccount(string account = "")
    {
        var trimmedAccount = account.Trim();

        var command = trimmedAccount.Length > 0 ? $"account get --account \"{trimmedAccount}\"" : "account get";
        return Op<AccountDetails>(command);
    }
    
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

        Op(command, password, false, false);

        _account = trimmedShorthand.Length > 0 ? trimmedShorthand : trimmedAddress;
    }

    public void UseAccount(string account)
    {
        var trimmedAccount = account.Trim();
        if (trimmedAccount.Length == 0)
            throw new ArgumentException($"{nameof(account)} cannot be empty.", nameof(account));

        _account = trimmedAccount;
    }

    public void SignIn(string password)
    {
        if (password.Length == 0)
            throw new ArgumentException($"{nameof(password)} cannot be empty.", nameof(password));

        var command = "signin --force --raw";
        var result = Op(command, password, true, false);
        _session = result.Trim();
    }

    public void SignOut(bool all = false)
    {
        var command = "signout";
        if (all)
            command += " --all";
        Op(command, !all, false);
    }

    public ImmutableList<string> ForgetAccount(bool all = false)
    {
        var accounts = ImmutableList.CreateBuilder<string>();

        var command = "account forget";
        command += all ? " --all" : $" \"{_account}\"";

        var result = Op(command, false, false);

        if (all)
            foreach (var match in ForgottenAccountsRegex.Matches(result).Cast<Match>())
                accounts.Add(match.Groups[1].Value);
        else
            accounts.Add(_account);

        return accounts.ToImmutable();
    }
}