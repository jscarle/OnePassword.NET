using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(99)]
public class TearDownAccount : TestsBase
{
    [Test]
    [Order(1)]
    public void SignOut()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.TearDown, () => { OnePassword.SignOut(true); });
    }

    [Test]
    [Order(2)]
    public void ForgetAccount()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.TearDown, () => { OnePassword.ForgetAccount(true); });
        DoFinalTearDown = true;
    }
}
