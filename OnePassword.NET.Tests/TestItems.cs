using OnePassword.Common;
using OnePassword.Items;

namespace OnePassword;

[TestFixture]
[Order(6)]
public class TestItems : TestsBase
{
    private const string InitialTitle = "Created Item";
    private const string InitialUsername = "Created Username";
    private const string EditSection = "Edit Section";
    private const string EditField = "Edit Field";
    private const string DeleteSection = "Delete Section";
    private const string DeleteField = "Delete Field";
    private const string DeleteValue = "Delete Value";
    private const FieldType InitialType = FieldType.String;
    private const string InitialValue = "Created Value";
    private Item _initialItem = null!;
    private const string FinalTitle = "Test Item";
    private const string FinalUsername = "Test Username";
    private const FieldType FinalType = FieldType.Concealed;
    private const string FinalValue = "Test Value";

    [Test]
    [Order(1)]
    public void CreateItem()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var template = TestTemplate.Clone();
            template.Title = InitialTitle;
            template.Fields.First(x => x.Label == "username").Value = InitialUsername;

            var editSection = new Section(EditSection);
            template.Sections.Add(editSection);
            template.Fields.Add(new Field(EditField, InitialType, InitialValue, editSection));

            var deleteSection = new Section(DeleteSection);
            template.Sections.Add(deleteSection);
            template.Fields.Add(new Field(DeleteField, FieldType.String, DeleteValue, deleteSection));

            _initialItem = OnePassword.CreateItem(template, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(_initialItem.Id, Is.Not.Empty);
                Assert.That(_initialItem.Title, Is.EqualTo(InitialTitle));
                Assert.That(_initialItem.Created, Is.Not.EqualTo(default));
                Assert.That(_initialItem.Fields.First(x => x.Label == "username").Value, Is.EqualTo(InitialUsername));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Type, Is.EqualTo(InitialType));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(InitialValue));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == DeleteSection && x.Label == DeleteField).Value, Is.EqualTo(DeleteValue));
            });
        });
    }

    [Test]
    [Order(2)]
    public void EditItem()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            _initialItem.Title = FinalTitle;
            _initialItem.Fields.First(x => x.Label == "username").Value = FinalUsername;
            _initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Type = FinalType;
            _initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value = FinalValue;
            _initialItem.Fields.Remove(_initialItem.Fields.First(x => x.Section?.Label == DeleteSection && x.Label == DeleteField));

            var item = OnePassword.EditItem(_initialItem, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.Not.Empty);
                Assert.That(item.Title, Is.EqualTo(FinalTitle));
                Assert.That(item.Created, Is.Not.EqualTo(default));
                Assert.That(item.Fields.First(x => x.Label == "username").Value, Is.EqualTo(FinalUsername));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Type, Is.EqualTo(FinalType));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(FinalValue));
                Assert.That(item.Fields.FirstOrDefault(x => x.Section?.Label == DeleteSection && x.Label == DeleteField), Is.Null);
            });
        });
    }

    [Test]
    [Order(3)]
    public void GetItems()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var items = OnePassword.GetItems(TestVault);

            Assert.That(items, Has.Count.GreaterThan(0));

            var item = items.First(x => x.Title == FinalTitle);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.Not.Empty);
                Assert.That(item.Title, Is.EqualTo(FinalTitle));
            });

            TestItem = item;
        });
    }

    [Test]
    [Order(4)]
    public void GetItem()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var item = OnePassword.GetItem(TestItem, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.Not.Empty);
                Assert.That(item.Title, Is.EqualTo(FinalTitle));
                Assert.That(item.Created, Is.Not.EqualTo(default));
                Assert.That(item.Fields.First(x => x.Label == "username").Value, Is.EqualTo(FinalUsername));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(FinalValue));
                Assert.That(item.Fields.FirstOrDefault(x => x.Section?.Label == DeleteSection && x.Label == DeleteField), Is.Null);
            });
        });
    }
}