using System.Diagnostics;
using OnePassword.Common;
using OnePassword.Users;

namespace OnePassword;

[TestFixture]
[Order(3)]
public class SetUpUser : TestsBase
{
    private const string InitialName = "Created User";
    private UserDetails _initialUser = null!;
    private const string FinalName = "Test User";

    [Test]
    [Order(1)]
    public void ProvisionUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            _initialUser = OnePassword.ProvisionUser(InitialName, TestUserEmail, Language.English);

            Assert.Multiple(() =>
            {
                Assert.That(_initialUser.Id, Is.Not.Empty);
                Assert.That(_initialUser.Name, Is.EqualTo(InitialName));
                Assert.That(_initialUser.Email, Is.EqualTo(TestUserEmail));
                Assert.That(_initialUser.Type, Is.EqualTo(UserType.Member));
                Assert.That(_initialUser.State, Is.EqualTo(State.TransferPending));
                Assert.That(_initialUser.Created, Is.Not.EqualTo(default));
                Assert.That(_initialUser.Updated, Is.Not.EqualTo(default));
                Assert.That(_initialUser.LastAuthentication, Is.EqualTo(null));
            });
        });
    }

    [Test]
    [Order(2)]
    public void ConfirmUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var currentState = State.Unknown;
            var stopWatch = Stopwatch.StartNew();
            while (currentState != State.TransferAccepted)
            {
                if (SetUpCancellationTokenSource.IsCancellationRequested)
                    throw new OperationCanceledException();

                if (stopWatch.ElapsedMilliseconds > TestUserConfirmTimeout)
                    throw new TimeoutException();

                Thread.Sleep(RateLimit * 10);
                currentState = OnePassword.GetUser(_initialUser).State;
            }
            stopWatch.Stop();
            OnePassword.ConfirmUser(_initialUser);
        });
    }

    [Test]
    [Order(3)]
    public void EditUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.EditUser(_initialUser, FinalName, false); });
    }

    [Test]
    [Order(4)]
    public void GetUsers()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var users = OnePassword.GetUsers();

            Assert.That(users, Has.Count.GreaterThan(0));

            var user = users.First(x => x.Name == FinalName);

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.Not.Empty);
                Assert.That(user.Name, Is.EqualTo(FinalName));
                Assert.That(user.Email, Is.EqualTo(TestUserEmail));
                Assert.That(user.Type, Is.EqualTo(UserType.Member));
                Assert.That(user.State, Is.EqualTo(State.Active));
            });

            TestUser = user;
        });
    }

    [Test]
    [Order(5)]
    public void GetUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var user = OnePassword.GetUser(TestUser);

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.Not.Empty);
                Assert.That(user.Name, Is.EqualTo(FinalName));
                Assert.That(user.Email, Is.EqualTo(TestUserEmail));
                Assert.That(user.Type, Is.EqualTo(UserType.Member));
                Assert.That(user.State, Is.EqualTo(State.Active));
                Assert.That(user.Created, Is.Not.EqualTo(default));
                Assert.That(user.Updated, Is.Not.EqualTo(default));
                Assert.That(user.LastAuthentication, Is.EqualTo(null));
            });
        });
    }

    [Test]
    [Order(6)]
    public void SuspendUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.SuspendUser(TestUser, 1); });
    }

    [Test]
    [Order(7)]
    public void ReactivateUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        Run(RunType.SetUp, () => { OnePassword.ReactivateUser(TestUser); });
    }
}
