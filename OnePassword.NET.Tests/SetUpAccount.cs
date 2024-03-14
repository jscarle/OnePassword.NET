using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(2)]
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
}
