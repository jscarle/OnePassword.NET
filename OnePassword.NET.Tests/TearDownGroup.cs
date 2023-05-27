using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(97)]
public class TearDownGroup : TestsBase
{
    [Test]
    [Order(1)]
    public void DeleteGroup()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.TearDown, () => { OnePassword.DeleteGroup(TestGroup); });
    }
}