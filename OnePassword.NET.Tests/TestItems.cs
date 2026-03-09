using OnePassword.Common;
using OnePassword.Items;

namespace OnePassword;

[TestFixture]
[Order(7)]
public class TestItems : TestsBase
{
    private const string InitialTitle = "Created Item";
    private const string InitialUsername = "Created Username";
    private const string InitialAttachmentName = "Initial Attachment";
    private const string InitialAttachmentContent = "Initial attachment content";
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
    private const string AddedField = "Added Field";
    private const string AddedValue = "Added Value";
    private const string AddedAttachmentName = "Added Attachment";
    private const string AddedAttachmentContent = "Added attachment content";
    private const string Tag1 = "Tag1";
    private const string Tag2 = "Tag2";
    private readonly string _addedAttachmentFilePath = Path.Combine(WorkingDirectory, "AddedAttachment.txt");
    private readonly string _addedAttachmentOutputFilePath = Path.Combine(WorkingDirectory, "AddedAttachment.out.txt");
    private readonly string _initialAttachmentFilePath = Path.Combine(WorkingDirectory, "InitialAttachment.txt");

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
            template.Tags.Add(Tag1);
            template.Tags.Add(Tag2);
            File.WriteAllText(_initialAttachmentFilePath, InitialAttachmentContent);
            template.FileAttachments.Add(new FileAttachment(_initialAttachmentFilePath, InitialAttachmentName));

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
                Assert.That(_initialItem.Created, Is.Not.EqualTo(default(DateTimeOffset?)));
                Assert.That(_initialItem.Fields.First(x => x.Label == "username").Value, Is.EqualTo(InitialUsername));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Type, Is.EqualTo(InitialType));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(InitialValue));
                Assert.That(_initialItem.Fields.First(x => x.Section?.Label == DeleteSection && x.Label == DeleteField).Value, Is.EqualTo(DeleteValue));
                Assert.That(_initialItem.FileAttachments.Any(x => x.Name == InitialAttachmentName), Is.True);
                Assert.That(_initialItem.FileAttachments.First(x => x.Name == InitialAttachmentName).ContentPath, Is.Not.Empty);
                Assert.That(_initialItem.FileAttachments.First(x => x.Name == InitialAttachmentName).Size, Is.GreaterThan(0));
                Assert.That(_initialItem.Tags, Does.Contain(Tag1));
                Assert.That(_initialItem.Tags, Does.Contain(Tag2));
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
            _initialItem.Tags.Remove(Tag2);

            var item = OnePassword.EditItem(_initialItem, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.Not.Empty);
                Assert.That(item.Title, Is.EqualTo(FinalTitle));
                Assert.That(item.Created, Is.Not.EqualTo(default(DateTimeOffset?)));
                Assert.That(item.Fields.First(x => x.Label == "username").Value, Is.EqualTo(FinalUsername));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Type, Is.EqualTo(FinalType));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(FinalValue));
                Assert.That(item.Fields.FirstOrDefault(x => x.Section?.Label == DeleteSection && x.Label == DeleteField), Is.Null);
                Assert.That(item.FileAttachments.Any(x => x.Name == InitialAttachmentName), Is.True);
                Assert.That(item.Tags, Does.Contain(Tag1));
                Assert.That(item.Tags, Does.Not.Contain(Tag2));
            });
        });
    }

    [Test]
    [Order(3)]
    public void EditItemAddsNewField()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var item = OnePassword.GetItem(_initialItem, TestVault);
            item.Fields.Add(new Field(AddedField, FieldType.String, AddedValue));

            var editedItem = OnePassword.EditItem(item, TestVault);

            Assert.That(editedItem.Fields.FirstOrDefault(x => x.Label == AddedField)?.Value, Is.EqualTo(AddedValue));
        });
    }

    [Test]
    [Order(4)]
    public void EditItemAddsFileAttachment()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            File.WriteAllText(_addedAttachmentFilePath, AddedAttachmentContent);

            var item = OnePassword.GetItem(_initialItem, TestVault);
            item.FileAttachments.Add(new FileAttachment(_addedAttachmentFilePath, AddedAttachmentName));

            var editedItem = OnePassword.EditItem(item, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(editedItem.FileAttachments.Any(x => x.Name == InitialAttachmentName), Is.True);
                Assert.That(editedItem.FileAttachments.Any(x => x.Name == AddedAttachmentName), Is.True);
            });

            _initialItem = editedItem;
        });
    }

    [Test]
    [Order(5)]
    public void EditItemRemovesFileAttachment()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.Test, () =>
        {
            var item = OnePassword.GetItem(_initialItem, TestVault);
            item.FileAttachments.Remove(item.FileAttachments.First(x => x.Name == InitialAttachmentName));

            var editedItem = OnePassword.EditItem(item, TestVault);

            Assert.Multiple(() =>
            {
                Assert.That(editedItem.FileAttachments.Any(x => x.Name == InitialAttachmentName), Is.False);
                Assert.That(editedItem.FileAttachments.Any(x => x.Name == AddedAttachmentName), Is.True);
            });

            _initialItem = editedItem;
        });
    }

    [Test]
    [Order(6)]
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
    [Order(7)]
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
                Assert.That(item.Created, Is.Not.EqualTo(default(DateTimeOffset?)));
                Assert.That(item.Fields.First(x => x.Label == "username").Value, Is.EqualTo(FinalUsername));
                Assert.That(item.Fields.First(x => x.Section?.Label == EditSection && x.Label == EditField).Value, Is.EqualTo(FinalValue));
                Assert.That(item.Fields.First(x => x.Label == AddedField).Value, Is.EqualTo(AddedValue));
                Assert.That(item.Fields.FirstOrDefault(x => x.Section?.Label == DeleteSection && x.Label == DeleteField), Is.Null);
                Assert.That(item.FileAttachments.Any(x => x.Name == InitialAttachmentName), Is.False);
                Assert.That(item.FileAttachments.Any(x => x.Name == AddedAttachmentName), Is.True);
            });

            var attachment = item.FileAttachments.First(x => x.Name == AddedAttachmentName);
            OnePassword.SaveFileAttachmentContent(attachment, item, TestVault, _addedAttachmentOutputFilePath);
            Assert.That(File.ReadAllText(_addedAttachmentOutputFilePath), Is.EqualTo(AddedAttachmentContent));
        });
    }
}
