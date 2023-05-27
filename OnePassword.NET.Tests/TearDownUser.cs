using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(98)]
public class TearDownUser : TestsBase
{
    [Test]
    [Order(1)]
    public void DeleteUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.TearDown, () => { OnePassword.DeleteUser(TestUser); });
    }
}