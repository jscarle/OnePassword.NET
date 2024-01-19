using OnePassword.Common;
using OnePassword.Groups;

namespace OnePassword;

[TestFixture]
[Order(4)]
public class SetUpGroup : TestsBase
{
    private const string InitialName = "Created Group";
    private const string InitialDescription = "Created by unit testing.";
    private GroupDetails _initialGroup = null!;
    private const string FinalName = "Test Group";
    private const string FinalDescription = "Used for unit testing.";

    [Test]
    [Order(1)]
    public void CreateGroup()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            _initialGroup = OnePassword.CreateGroup(InitialName, InitialDescription);

            Assert.Multiple(() =>
            {
                Assert.That(_initialGroup.Id, Is.Not.Empty);
                Assert.That(_initialGroup.Name, Is.EqualTo(InitialName));
                Assert.That(_initialGroup.Description, Is.EqualTo(InitialDescription));
                Assert.That(_initialGroup.Type, Is.EqualTo(GroupType.User));
                Assert.That(_initialGroup.State, Is.EqualTo(State.Inactive));
                Assert.That(_initialGroup.Created, Is.Not.EqualTo(default));
                Assert.That(_initialGroup.Updated, Is.Not.EqualTo(default));
                Assert.That(_initialGroup.Permissions, Has.Count.EqualTo(0));
            });
        });
    }

    [Test]
    [Order(2)]
    public void EditGroup()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.EditGroup(_initialGroup, FinalName, FinalDescription); });
    }

    [Test]
    [Order(3)]
    public void GetGroups()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var groups = OnePassword.GetGroups();

            Assert.That(groups, Has.Count.GreaterThan(0));

            var group = groups.First(x => x.Name == FinalName);

            Assert.Multiple(() =>
            {
                Assert.That(group.Id, Is.Not.Empty);
                Assert.That(group.Name, Is.EqualTo(FinalName));
            });

            TestGroup = group;
        });
    }

    [Test]
    [Order(4)]
    public void GetGroup()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var group = OnePassword.GetGroup(TestGroup);

            Assert.Multiple(() =>
            {
                Assert.That(group.Id, Is.Not.Empty);
                Assert.That(group.Name, Is.EqualTo(FinalName));
                Assert.That(group.Description, Is.EqualTo(FinalDescription));
                Assert.That(group.Type, Is.EqualTo(GroupType.User));
                Assert.That(group.State, Is.EqualTo(State.Active));
                Assert.That(group.Created, Is.Not.EqualTo(default));
                Assert.That(group.Updated, Is.Not.EqualTo(default));
                Assert.That(group.Permissions, Has.Count.EqualTo(0));
            });
        });
    }
}
