using System.Collections.Immutable;
using System.Text.RegularExpressions;
using OnePassword.Accounts;

namespace OnePassword;

public partial class OnePasswordManager
{
    private static readonly Regex ForgetAllAccountsRegex = new(@"""([^""]+)""", RegexOptions.Compiled);

    public ImmutableList<Account> GetAccounts()
    {
        return Op<ImmutableList<Account>>("account list", false, false);
    }

    public AccountDetails GetAccount()
    {
        return Op<AccountDetails>("account get");
    }

    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "")
    {
        shorthand = shorthand.Trim();

        var command = $"account add --address {address} --email {email} --secret-key {secretKey}";
        if (!string.IsNullOrEmpty(shorthand))
            command += $" --shorthand {shorthand}";

        Op(command, password, false, false);

        _account = !string.IsNullOrEmpty(shorthand) ? shorthand : address;
    }

    public void UseAccount(string account)
    {
        _account = account;
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
            foreach (var match in ForgetAllAccountsRegex.Matches(result).Cast<Match>())
                accounts.Add(match.Groups[1].Value);
        else
            accounts.Add(_account);

        return accounts.ToImmutable();
    }
}