using OnePassword.NET.Tests.Common;
using OnePassword.Vaults;

namespace OnePassword.NET.Tests;

[TestFixture, Order(4)]
public class SetUpVault : TestsBase
{
    private const string InitialName = "Created Vault";
    private const string InitialDescription = "Created by unit testing.";
    private const VaultIcon InitialIcon = VaultIcon.Gears;
    private VaultDetails _initialVault = null!;
    private const string FinalName = "Test Vault";
    private const string FinalDescription = "Used for unit testing.";
    private const VaultIcon FinalIcon = VaultIcon.Airplane;

    [Test, Order(1)]
    public void CreateVault()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            _initialVault = OnePassword.CreateVault(InitialName, InitialDescription, InitialIcon, true);
            Assert.Multiple(() =>
            {
                Assert.That(_initialVault.Id, Is.Not.Empty);
                Assert.That(_initialVault.Name, Is.EqualTo(InitialName));
                Assert.That(_initialVault.Type, Is.EqualTo(VaultType.User));
                Assert.That(_initialVault.Items, Is.EqualTo(0));
                Assert.That(_initialVault.Created, Is.Not.EqualTo(default));
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
    public void EditVault()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            OnePassword.EditVault(_initialVault, FinalName, FinalDescription, FinalIcon, false);
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
    public void GetVaults()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var vaults = OnePassword.GetVaults();
            Assert.That(vaults, Has.Count.GreaterThan(0));

            var vault = vaults.First(x => x.Name == FinalName);
            Assert.Multiple(() =>
            {
                Assert.That(vault.Id, Is.Not.Empty);
                Assert.That(vault.Name, Is.EqualTo(FinalName));
            });

            TestVault = vault;
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
    public void GetVault()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var vaultDetails = OnePassword.GetVault(TestVault);
            Assert.Multiple(() =>
            {
                Assert.That(vaultDetails.Id, Is.Not.Empty);
                Assert.That(vaultDetails.Name, Is.EqualTo(FinalName));
                Assert.That(vaultDetails.Type, Is.EqualTo(VaultType.User));
                Assert.That(vaultDetails.Items, Is.EqualTo(0));
                Assert.That(vaultDetails.Created, Is.Not.EqualTo(default));
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