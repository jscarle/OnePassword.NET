using OnePassword.Accounts;
using OnePassword.Common;
using OnePassword.NET.Tests.Common;

namespace OnePassword.NET.Tests;

[TestFixture, Order(1)]
public class SetUpAccount : TestsBase
{
    [Test, Order(1)]
    public void AddAccount()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.AddAccount(AccountAddress, AccountEmail, AccountSecretKey, AccountPassword);
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
    public void SignIn()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.SignIn(AccountPassword);
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
    public void GetAccounts()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
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
    public void GetAccount()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
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