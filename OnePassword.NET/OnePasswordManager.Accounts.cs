using System.Collections.Immutable;
using System.Text.RegularExpressions;
using OnePassword.Accounts;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly Regex ForgottenAccountsRegex = new(@"""([^""]+)""", RegexOptions.Compiled);

    public ImmutableList<Account> GetAccounts()
    {
        return Op<ImmutableList<Account>>("account list", false, false);
    }

    public AccountDetails GetAccount(string account)
    {
        var trimmedAccount = account.Trim();

        return Op<AccountDetails>(!string.IsNullOrWhiteSpace(trimmedAccount) ? $"account get --account {trimmedAccount}" : "account get");
    }
    
    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "")
    {
        var trimmedAddress = address.Trim();
        var trimmedEmail = email.Trim();
        var trimmedSecretKey = secretKey.Trim();
        var trimmedShorthand = shorthand.Trim();

        var command = $"account add --address {trimmedAddress} --email {trimmedEmail} --secret-key {trimmedSecretKey}";
        if (!string.IsNullOrEmpty(trimmedShorthand))
            command += $" --shorthand {trimmedShorthand}";

        Op(command, password, false, false);

        _account = !string.IsNullOrEmpty(trimmedShorthand) ? trimmedShorthand : trimmedAddress;
    }

    public void UseAccount(string account)
    {
        var trimmedAccount = account.Trim();

        _account = trimmedAccount;
    }

    public void SignIn(string password)
    {
        _session = Op("signin --raw", password, true, false);
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
        command += all ? " --all" : $" {_account}";

        var result = Op(command, false, false);

        if (all)
            foreach (var match in ForgottenAccountsRegex.Matches(result).Cast<Match>())
                accounts.Add(match.Groups[1].Value);
        else
            accounts.Add(_account);

        return accounts.ToImmutable();
    }
}