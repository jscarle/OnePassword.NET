using OnePassword.Accounts;
using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(1)]
public class SetUpAccount : TestsBase
{
    [Test]
    [Order(1)]
    public void AddAccount()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.AddAccount(AccountAddress, AccountEmail, AccountSecretKey, AccountPassword); });
    }

    [Test]
    [Order(2)]
    public void SignIn()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.SignIn(AccountPassword); });
    }

    [Test]
    [Order(3)]
    public void GetAccounts()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var accounts = OnePassword.GetAccounts();

            Assert.That(accounts, Has.Count.EqualTo(1));

            var account = accounts.First();

            Assert.Multiple(() =>
            {
                Assert.That(account.Id, Is.Not.Empty);
                Assert.That(account.Url, Is.EqualTo($"https://{AccountAddress}"));
                Assert.That(account.UserId, Is.Not.Empty);
                Assert.That(account.Email, Is.EqualTo(AccountEmail));
                Assert.That(account.Shorthand, Is.EqualTo(AccountAddress[..AccountAddress.IndexOf(".", StringComparison.Ordinal)]));
            });
        });
    }

    [Test]
    [Order(4)]
    public void GetAccount()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var account = OnePassword.GetAccount();

            Assert.Multiple(() =>
            {
                Assert.That(account.Id, Is.Not.Empty);
                Assert.That(account.Name, Is.EqualTo(AccountName));
                Assert.That(account.Domain, Is.EqualTo(AccountAddress[..AccountAddress.IndexOf(".", StringComparison.Ordinal)]));
                Assert.That(account.Type, Is.EqualTo(AccountType.Business));
                Assert.That(account.State, Is.EqualTo(State.Active));
                Assert.That(account.Created, Is.Not.EqualTo(default));
            });
        });
    }
}