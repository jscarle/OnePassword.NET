using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using OnePassword.Documents;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private readonly string _opPath;
    private readonly bool _verbose;
    private string _account = "";
    private string _session = "";

    public OnePasswordManager(string path = "", string executable = "op.exe", bool verbose = false)
    {
        _opPath = path.Length > 0 ? Path.Combine(path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
        if (!File.Exists(_opPath))
            throw new Exception($"The 1Password CLI executable ({executable}) was not found folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = verbose;
    }

    public void ArchiveDocument(Document document) => ArchiveDocument(document, null);

    public void ArchiveDocument(Document document, Vault? vault)
    {
        var command = $"document delete \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        command += " --archive";
        Op(command);
    }

    public void ArchiveItem(Item item) => ArchiveItem(item, null);

    public void ArchiveItem(Item item, Vault? vault)
    {
        var command = $"item delete \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        command += " --archive";
        Op(command);
    }

    public Document CreateDocument(Template template, string path) => CreateDocument(template, null, path);

    public Document CreateDocument(Template template, Vault? vault, string path)
    {
        if (template.Uuid != "006") // Document
            throw new ArgumentException("Cannot create an Item using this method. Use CreateItem instead.");

        var command = $"document create \"{path}\"";
        if (!string.IsNullOrEmpty(template.Title))
            command += $" --title \"{template.Title}\"";
        else
            command += $" --title \"{template.Name}\"";
        if (!string.IsNullOrEmpty(template.Filename))
            command += $" --file-name \"{template.Filename}\"";
        if (template.Tags.Count > 0)
            command += $" --tags \"{string.Join(",", template.Tags.ToArray())}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        return Op<Document>(command);
    }

    public Item CreateItem(Template template) => CreateItem(template, null);

    public Item CreateItem(Template template, Vault? vault)
    {
        if (template.Uuid == "006") // Document
            throw new ArgumentException("Cannot create a Document using this method. Use CreateDocument instead.");

        var command = $"item create \"{template.Name}\" \"{template.Details.ToBase64()}\"";
        if (!string.IsNullOrEmpty(template.Title))
            command += $" --title \"{template.Title}\"";
        else
            command += $" --title \"{template.Name}\"";
        if (template.PasswordRecipe != null && (template.Uuid == "001" || template.Uuid == "005")) // Logor Password
        {
            command += " --generate-password=";
            if (template.PasswordRecipe.Value.Letters)
                command += "letters,";
            if (template.PasswordRecipe.Value.Digits)
                command += "digits,";
            if (template.PasswordRecipe.Value.Symbols)
                command += "symbols,";
            command += $"{template.PasswordRecipe.Value.Length}";
        }
        if (!string.IsNullOrEmpty(template.Url))
            command += $" --url \"{template.Url}\"";
        if (template.Tags.Count > 0)
            command += $" --tags \"{string.Join(",", template.Tags.ToArray())}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        return Op<Item>(command);
    }

    public void DeleteDocument(Document document) => DeleteDocument(document, null);

    public void DeleteDocument(Document document, Vault? vault)
    {
        var command = $"document delete \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        Op(command);
    }

    public void DeleteItem(Item item) => DeleteItem(item, null);

    public void DeleteItem(Item item, Vault? vault)
    {
        var command = $"item delete \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        Op(command);
    }

    public void GetDocument(Item document, string path) => GetDocument(document, null, path);

    public void GetDocument(Item document, Vault? vault, string path)
    {
        var command = $"document get \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        command += $" --output \"{path}\"";
        Op(command);
    }

    public Item GetItem(Item item) => GetItem(item, null);

    public Item GetItem(Item item, Vault? vault)
    {
        var command = $"item get \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        return Op<Item>(command);
    }

    public Template GetTemplate(Template template)
    {
        var command = $"item template gate \"{template.Name}\"";
        template.Details = Op<ItemDetails>(command);
        return template;
    }

    public DocumentList ListDocuments(bool includeTrash = false) => ListDocuments(null, includeTrash);

    public DocumentList ListDocuments(Vault? vault, bool includeTrash = false)
    {
        var command = "document list";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        if (includeTrash)
            command += " --include-trash";
        return Op<DocumentList>(command);
    }

    public ItemList ListItems(bool includeTrash = false) => ListItems(null, null, "", includeTrash);

    public ItemList ListItems(Vault? vault, bool includeTrash = false) => ListItems(vault, null, "", includeTrash);

    public ItemList ListItems(Vault? vault, Template? template, bool includeTrash = false) => ListItems(vault, template, "", includeTrash);

    public ItemList ListItems(Vault? vault, string tag, bool includeTrash = false) => ListItems(vault, null, tag, includeTrash);

    public ItemList ListItems(Template? template, bool includeTrash = false) => ListItems(null, template, "", includeTrash);

    public ItemList ListItems(Template? template, string tag, bool includeTrash = false) => ListItems(null, template, tag, includeTrash);

    public ItemList ListItems(string tag, bool includeTrash = false) => ListItems(null, null, tag, includeTrash);

    public ItemList ListItems(Vault? vault, Template? template, string tag, bool includeTrash = false)
    {
        var command = "item list";
        if (vault != null)
            command += $" --vault \"{vault.Id}\"";
        if (template != null)
            command += $" --categories \"{template.Name}\"";
        if (!string.IsNullOrEmpty(tag))
            command += $" --tags \"{tag}\"";
        if (includeTrash)
            command += " --include-trash";
        return Op<ItemList>(command);
    }

    public TemplateList ListTemplates()
    {
        var command = "item template list";
        return Op<TemplateList>(command);
    }

#if !NET40 && !NET35 && !NET20
    public bool Update()
    {
        var updated = false;

        var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDirectory);

        var command = $"update --directory \"{tempDirectory}\"";
        var result = Op(command);

        if (Regex.Match(result, @"Version ([^\s]+) is now available\.").Success)
            foreach (var file in Directory.GetFiles(tempDirectory, "*.zip"))
            {
                using var zipArchive = ZipFile.Open(file, ZipArchiveMode.Read);

                var entry = zipArchive.GetEntry("op.exe");
                if (entry is null)
                    continue;
                entry.ExtractToFile(_opPath, true);

                updated = true;
            }

        Directory.Delete(tempDirectory, true);

        return updated;
    }
#endif

    private static string GetStandardError(Process process)
    {
        var error = new StringBuilder();
        while (process.StandardError.Peek() > -1)
            error.Append((char)process.StandardError.Read());
        return error.ToString();
    }

    private static string GetStandardOutput(Process process)
    {
        var output = new StringBuilder();
        while (process.StandardOutput.Peek() > -1)
            output.Append((char)process.StandardOutput.Read());
        return output.ToString();
    }

    private TResult Op<TResult>(string command, bool passAccount = true, bool passSession = true, bool returnError = false)
        where TResult : class
    {
        var result = Op(command, passAccount, passSession, returnError);
        return JsonSerializer.Deserialize<TResult>(result) ?? throw new Exception("Could not deserialize the command result.");
    }

    private string Op(string command, bool passAccount = true, bool passSession = true, bool returnError = false)
    {
        return Op(command, Array.Empty<string>(), passAccount, passSession, returnError);
    }

    private string Op(string command, string input, bool passAccount = true, bool passSession = true, bool returnError = false)
    {
        return Op(command, new[] { input }, passAccount, passSession, returnError);
    }

    private string Op(string command, IEnumerable<string> input, bool passAccount = true, bool passSession = true, bool returnError = false)
    {
        if (passAccount && _account.Length == 0)
            throw new Exception("Cannot execute command because account has not been set.");
        if (passSession && _session.Length == 0)
            throw new Exception("Cannot execute command because account has not been signed in.");

        var arguments = command;
        if (passAccount)
            arguments += $" --account {_account}";
        if (passSession)
            arguments += $" --session {_session}";

        if (_verbose)
            Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

        var process = Process.Start(new ProcessStartInfo(_opPath, $"{arguments} --format json --no-color")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        });

        if (process is null)
            throw new Exception($"Could not start process for {_opPath}.");

        foreach (var inputLine in input)
        {
            process.StandardInput.WriteLine(inputLine);
            process.StandardInput.Flush();
        }

        var output = GetStandardOutput(process);
        if (_verbose)
            Console.WriteLine(output);

        var error = GetStandardError(process);
        if (_verbose)
            Console.WriteLine(error);

        if (!error.StartsWith("[ERROR]"))
            return output;

        if (returnError)
            return error;

        throw new Exception(error.Length > 28 ? error[28..].Trim() : error);
    }
}