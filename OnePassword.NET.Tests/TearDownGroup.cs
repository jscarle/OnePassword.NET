using OnePassword.NET.Tests.Common;

namespace OnePassword.NET.Tests;

[TestFixture, Order(97)]
public class TearDownGroup : TestsBase
{
    [Test, Order(1)]
    public void DeleteGroup()
    {
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