using OnePassword.Common;
using OnePassword.Documents;

namespace OnePassword;

[TestFixture]
[Order(7)]
public class TestDocuments : TestsBase
{
    private string _filePath = null!;
    private const string InitialFileName = "CreatedDocument.txt";
    private const string InitialTitle = "Created Document";
    private const string InitialFileContent = "Created by unit testing.";
    private Document _initialDocument = null!;
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

            _initialDocument = OnePassword.CreateDocument(TestVault, _filePath, InitialFileName, InitialTitle);

            Assert.Multiple(() => { Assert.That(_initialDocument.Id, Is.Not.Empty); });
        });
    }

    [Test]
    [Order(2)]
    public void ReplaceDocument()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        File.WriteAllText(_filePath, FinalFileContent);

        Run(RunType.SetUp, () => { OnePassword.ReplaceDocument(_initialDocument, TestVault, _filePath, FinalFileName, FinalTitle); });
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
            var documents = OnePassword.GetDocuments(TestVault);

            Assert.That(documents, Has.Count.GreaterThan(0));

            var document = documents.First(x => x.Title == FinalTitle);

            Assert.Multiple(() =>
            {
                Assert.That(document.Id, Is.EqualTo(_initialDocument.Id));
                Assert.That(document.Title, Is.EqualTo(FinalTitle));
            });

            TestDocument = document;
        });
    }

    [Test]
    [Order(4)]
    public void GetDocument()
    {
        if (!RunLiveTests)
            Assert.Ignore();

        var outFilePath = Path.Combine(WorkingDirectory, "OutFile-FileNameOnDisk.txt");

        Run(RunType.SetUp, () =>
        {
            OnePassword.GetDocument(_initialDocument, TestVault, outFilePath);

            var documentContent = File.ReadAllText(outFilePath);

            Assert.That(documentContent, Is.EqualTo(FinalFileContent));
        });
    }
}