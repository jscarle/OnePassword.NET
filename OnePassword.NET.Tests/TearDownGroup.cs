using OnePassword.Common;

namespace OnePassword;

[TestFixture, Order(97)]
public class TearDownGroup : TestsBase
{
    [Test, Order(1)]
    public void DeleteGroup()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, TearDownCancellationTokenSource.Token);
        try
        {
            OnePassword.DeleteGroup(TestGroup);
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