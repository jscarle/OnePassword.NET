using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;

namespace OnePassword;

[TestFixture]
[Order(5)]
public class TestTemplates : TestsBase
{
    private ITemplate _template = null!;

    [Test]
    [Order(1)]
    public void GetTemplates()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var templates = OnePassword.GetTemplates();

            Assert.That(templates, Has.Count.EqualTo(22));

            _template = templates.First(x => x.Name == "Login");
        });
    }

    [Test]
    [Order(2)]
    public void GetTemplatesName()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            TestTemplate = OnePassword.GetTemplate(_template);

            Assert.Multiple(() =>
            {
                Assert.That(TestTemplate.Name, Is.EqualTo(_template.Name));
                Assert.That(TestTemplate.Category, Is.Not.EqualTo(Category.Unknown));
            });
        });
    }

    [Test]
    [Order(3)]
    public void GetTemplatesByEnum()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
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
        });
    }
}