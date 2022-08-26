using OnePassword.Common;
using OnePassword.NET.Tests.Common;
using OnePassword.Users;

namespace OnePassword.NET.Tests;

[TestFixture, Order(2)]
public class SetUpUser : TestsBase
{
    private const string InitialName = "Created User";
    private UserDetails _initialUser = null!;
    private const string FinalName = "Test User";

    [Test, Order(1)]
    public void ProvisionUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            _initialUser = OnePassword.ProvisionUser(InitialName, TestEmail, Language.English);
            Assert.Multiple(() =>
            {
                Assert.That(_initialUser.Id, Is.Not.Empty);
                Assert.That(_initialUser.Name, Is.EqualTo(InitialName));
                Assert.That(_initialUser.Email, Is.EqualTo(TestEmail));
                Assert.That(_initialUser.Type, Is.EqualTo(UserType.Member));
                Assert.That(_initialUser.State, Is.EqualTo(State.TransferPending));
                Assert.That(_initialUser.Created, Is.Not.EqualTo(default));
                Assert.That(_initialUser.Updated, Is.Not.EqualTo(default));
                Assert.That(_initialUser.LastAuthentication, Is.EqualTo(null));
            });
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(2)]
    public void ConfirmUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var currentState = State.Unknown;
            while (currentState != State.TransferAccepted)
            {
                if (SetUpCancellationTokenSource.IsCancellationRequested)
                    throw new OperationCanceledException();

                Thread.Sleep(RateLimit * 10);
                currentState = OnePassword.GetUser(_initialUser).State;
            }
            OnePassword.ConfirmUser(_initialUser);
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(3)]
    public void EditUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.EditUser(_initialUser, FinalName, false);
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(4)]
    public void GetUsers()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var users = OnePassword.GetUsers();
            Assert.That(users, Has.Count.GreaterThan(0));

            var user = users.First(x => x.Name == FinalName);
            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.Not.Empty);
                Assert.That(user.Name, Is.EqualTo(FinalName));
                Assert.That(user.Email, Is.EqualTo(TestEmail));
                Assert.That(user.Type, Is.EqualTo(UserType.Member));
                Assert.That(user.State, Is.EqualTo(State.Active));
            });

            TestUser = user;
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(5)]
    public void GetUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var userDetails = OnePassword.GetUser(TestUser);
            Assert.Multiple(() =>
            {
                Assert.That(userDetails.Id, Is.Not.Empty);
                Assert.That(userDetails.Name, Is.EqualTo(FinalName));
                Assert.That(userDetails.Email, Is.EqualTo(TestEmail));
                Assert.That(userDetails.Type, Is.EqualTo(UserType.Member));
                Assert.That(userDetails.State, Is.EqualTo(State.Active));
                Assert.That(userDetails.Created, Is.Not.EqualTo(default));
                Assert.That(userDetails.Updated, Is.Not.EqualTo(default));
                Assert.That(userDetails.LastAuthentication, Is.EqualTo(null));
            });
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(6)]
    public void SuspendUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.SuspendUser(TestUser, 1);
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(7)]
    public void ReactivateUser()
    {
        if (!CreateTestUser)
            Assert.Inconclusive();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.ReactivateUser(TestUser);
        }
        catch (Exception)
        {
            SetUpCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }
}