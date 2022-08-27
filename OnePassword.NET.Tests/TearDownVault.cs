using OnePassword.NET.Tests.Common;

namespace OnePassword.NET.Tests;

[TestFixture, Order(96)]
public class TearDownVault : TestsBase
{
    [Test, Order(1)]
    public void DeleteVault()
    {
        SemaphoreSlim.Wait(CommandTimeout, TearDownCancellationTokenSource.Token);
        try
        {
            OnePassword.DeleteVault(TestVault);
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
}