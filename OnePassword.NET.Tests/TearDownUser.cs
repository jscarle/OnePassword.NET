using OnePassword.NET.Tests.Common;

namespace OnePassword.NET.Tests;

[TestFixture, Order(98)]
public class TearDownUser : TestsBase
{
    [Test, Order(1)]
    public void DeleteUser()
    {
        if (!RunLiveTests || !CreateTestUser)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, TearDownCancellationTokenSource.Token);
        try
        {
            OnePassword.DeleteUser(TestUser);
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