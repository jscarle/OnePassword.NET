using OnePassword.Common;

namespace OnePassword;

[TestFixture]
[Order(95)]
public class TearDownDocument : TestsBase
{
    [Test]
    [Order(1)]
    public void DeleteDocument()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.TearDown, () => 
        {
            // Tests Archive (both listing and deleting), followed by permanent deletion

            OnePassword.DeleteDocument(TestDocument.Id, archive: true, vault: TestVault.Id);

            var documents = OnePassword.GetDocuments(vault: TestVault.Id, includeArchive: true);

            Assert.That(documents, Has.Count.GreaterThan(0));

            var document = documents.First(x => x.Id == TestDocument.Id);

            Assert.Multiple(() =>
            {
                Assert.That(document.Id, Is.EqualTo(TestDocument.Id));
            });

            OnePassword.DeleteDocument(TestDocument.Id, archive: false, vault: TestVault.Id);

            documents = OnePassword.GetDocuments(vault: TestVault.Id, includeArchive: true);

            Assert.That(documents, Is.Empty);
        });
    }
}