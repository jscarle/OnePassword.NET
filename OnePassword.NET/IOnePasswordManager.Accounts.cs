using OnePassword.Accounts;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets the accounts.</summary>
    /// <returns>The list of accounts.</returns>
    public ImmutableList<Account> GetAccounts();

    /// <summary>Gets the account details.</summary>
    /// <param name="account">The account to retrieve.</param>
    /// <returns>The account details.</returns>
    public AccountDetails GetAccount(string account = "");

    /// <summary>Adds an account.</summary>
    /// <param name="address">The account address.</param>
    /// <param name="email">The account email.</param>
    /// <param name="secretKey">The account secret key.</param>
    /// <param name="password">The account password.</param>
    /// <param name="shorthand">The account shorthand.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "");

    /// <summary>Uses the account.</summary>
    /// <param name="account">The account to use.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void UseAccount(string account);

    /// <summary>Signs in to the account.</summary>
    /// <param name="password">The account password to use when manually signing in.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SignIn(string? password = null);

    /// <summary>Signs out of the account.</summary>
    /// <param name="all">When <see langword="true" />, signs out of all accounts.</param>
    public void SignOut(bool all = false);

    /// <summary>Forgets the account.</summary>
    /// <param name="all">When <see langword="true" />, forgets all accounts.</param>
    /// <returns>The list of accounts that were forgotten.</returns>
    public ImmutableList<string> ForgetAccount(bool all = false);
}
