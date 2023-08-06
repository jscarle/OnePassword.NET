using OnePassword.Common;
using OnePassword.Documents;

namespace OnePassword;

[TestFixture]
[Order(7)]
public class SetUpDocument : TestsBase
{
    private string _filePath = null!;
    private const string InitialFileName = "CreatedDocument.txt";
    private const string InitialTitle = "Created Document";
    private const string InitialFileContent = "Created by unit testing.";
    private CreateDocument _initialCreateDocument = null!;
    private const string FinalFileName = "TestDocument.Txt";
    private const string FinalTitle = "Test Document";
    private const string FinalFileContent = "Test for unit testing.";

    [Test]
    [Order(1)]
    public void CreateDocument()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            _filePath = Path.Combine(WorkingDirectory, "FileNameOnDisk.txt");
            File.WriteAllText(_filePath, InitialFileContent);

            // Does not return title or filename
            _initialCreateDocument = OnePassword.CreateDocument(
                _filePath,
                InitialFileName,
                InitialTitle,
                TestVault.Name);

            Assert.Multiple(() =>
            {
                // Returns "uuid" instead of "id"
                Assert.That(_initialCreateDocument.VaultUUId, Is.EqualTo(TestVault.Id));
                Assert.That(_initialCreateDocument.UUId, Is.Not.Empty);
                Assert.That(_initialCreateDocument.Created, Is.Not.EqualTo(default));
                Assert.That(_initialCreateDocument.Updated, Is.Not.EqualTo(default));
            });
        });
    }

    [Test]
    [Order(2)]
    public void EditDocument()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        File.WriteAllText(_filePath, FinalFileContent);

        Run(RunType.SetUp, () => { OnePassword.EditDocument(
            _initialCreateDocument.UUId,
            _filePath,
            FinalFileName,
            FinalTitle,
            TestVault.Name); 
        });
    }

    [Test]
    [Order(3)]
    public void GetDocuments()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            // Does not return FileName
            var documents = OnePassword.GetDocuments();

            Assert.That(documents, Has.Count.GreaterThan(0));

            var document = documents.First(x => x.Title == FinalTitle);

            Assert.Multiple(() =>
            {
                Assert.That(document.Id, Is.EqualTo(_initialCreateDocument.UUId));
                Assert.That(document.Title, Is.EqualTo(FinalTitle));
            });

            TestDocument = document;
        });
    }

    [Test]
    [Order(4)]
    public void GetDocumentResponse()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        Run(RunType.SetUp, () =>
        {
            var documentContent = OnePassword.GetDocument(_initialCreateDocument.UUId, vault: TestVault.Name);

            Assert.That(documentContent, Is.EqualTo(FinalFileContent));
        });
    }

    [Test]
    [Order(5)]
    public void GetDocumentFile()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        var outFilePath = Path.Combine(WorkingDirectory, "OutFile-FileNameOnDisk.txt");

        Run(RunType.SetUp, () =>
        {
            OnePassword.GetDocument(
                _initialCreateDocument.UUId,
                outFile: outFilePath,
                vault: TestVault.Name);

            var documentContent = File.ReadAllText(outFilePath);

            Assert.That(documentContent, Is.EqualTo(FinalFileContent));
        });
    }
}