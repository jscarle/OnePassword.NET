using OnePassword.NET.Tests.Common;

namespace OnePassword.NET.Tests;

[TestFixture, Order(99)]
public class TearDownAccount : TestsBase
{
    [Test, Order(1)]
    public void SignOut()
    {
        SemaphoreSlim.Wait(CommandTimeout, TearDownCancellationTokenSource.Token);
        try
        {
            OnePassword.SignOut(true);
        }
        catch (Exception)
        {
            TearDownCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
    }

    [Test, Order(2)]
    public void ForgetAccount()
    {
        SemaphoreSlim.Wait(CommandTimeout, TearDownCancellationTokenSource.Token);
        try
        {
            OnePassword.ForgetAccount(true);
        }
        catch (Exception)
        {
            TearDownCancellationTokenSource.Cancel();
            throw;
        }
        finally
        {
            Thread.Sleep(RateLimit);
            SemaphoreSlim.Release();
        }
        DoFinalTearDown = true;
    }
}