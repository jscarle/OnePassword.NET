using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(96)]
public class TearDownVault : TestsBase
{
    [Test]
    [Order(1)]
    public void DeleteVault()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.TearDown, () => { OnePassword.DeleteVault(TestVault); });
    }
}