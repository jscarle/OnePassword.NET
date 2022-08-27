using OnePassword.Items;
using OnePassword.NET.Tests.Common;
using OnePassword.Templates;

namespace OnePassword.NET.Tests;

[TestFixture, Order(5)]
public class TestTemplates : TestsBase
{
    private ITemplate _template = null!;

    [Test, Order(1)]
    public void GetTemplates()
    {
        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var templates = OnePassword.GetTemplates();
            Assert.That(templates, Has.Count.GreaterThan(0));

            _template = templates.First();
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
    public void GetTemplatesName()
    {
        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            var template = OnePassword.GetTemplate(_template);
            Assert.Multiple(() =>
            {
                Assert.That(template.Name, Is.EqualTo(_template.Name));
                Assert.That(template.Category, Is.Not.EqualTo(Category.Unknown));
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

    [Test, Order(3)]
    public void GetTemplatesByEnum()
    {
        SemaphoreSlim.Wait(CommandTimeout, SetUpCancellationTokenSource.Token);
        try
        {
            foreach (var enumValue in Enum.GetValues(typeof(Category)))
            {
                var enumMember = (Category)enumValue;
                if (enumMember is Category.Custom or Category.Unknown)
                    continue;

                var enumMemberString = enumMember.ToEnumString();

                var template = OnePassword.GetTemplate(enumMember);
                Assert.Multiple(() =>
                {
                    Assert.That(template.Name, Is.EqualTo(enumMemberString));
                    Assert.That(template.Category, Is.Not.EqualTo(Category.Unknown));
                });

                Thread.Sleep(RateLimit);
            }
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