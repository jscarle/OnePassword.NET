using OnePassword.Common;

namespace OnePassword;

[TestFixture, Order(96)]
public class TearDownVault : TestsBase
{
    [Test, Order(1)]
    public void DeleteVault()
    {
        if (!RunLiveTests)
            Assert.Ignore();

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