using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using OnePassword.Documents;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;
using Group = OnePassword.Groups.Group;

namespace OnePassword;

public partial class OnePasswordManager
{
    private readonly string _opPath;
    private readonly bool _verbose;
    private string _account = "";
    private string _session = "";

    public OnePasswordManager(string path = "", string executable = "op.exe", bool verbose = false)
    {
        _opPath = !string.IsNullOrEmpty(path) ? Path.Combine(path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
        if (!File.Exists(_opPath))
            throw new Exception($"The 1Password CLI executable ({executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

        _verbose = verbose;
    }

    public void AddGroup(Group group, Vault vault, string permission = "allow_editing,allow_viewing,allow_managing") => Op($"vault group grant --vault \"{vault.Uuid}\" --group \"{group.Uuid}\" --permission \"{permission}\"");

    public void AddUser(User user, Vault vault, string permission = "allow_editing,allow_viewing,allow_managing") => Op($"vault user grant --vault \"{vault.Uuid}\" --user \"{user.Uuid}\" --permission \"{permission}\"");

    public void AddUser(User user, Group group, UserRole userRole = UserRole.Member)
    {
        var command = $"group user grant --group \"{group.Uuid}\" --user \"{user.Uuid}\"";
        if (userRole == UserRole.Manager)
            command += " --role manager";
        Op(command);
    }

    public void ArchiveDocument(Document document) => ArchiveDocument(document, null);

    public void ArchiveDocument(Document document, Vault? vault)
    {
        var command = $"document delete \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        command += " --archive";
        Op(command);
    }

    public void ArchiveItem(Item item) => ArchiveItem(item, null);

    public void ArchiveItem(Item item, Vault? vault)
    {
        var command = $"item delete \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        command += " --archive";
        Op(command);
    }

    public void ConfirmUser(User user) => Op($"user confirm \"{user.Uuid}\"");

    public void ConfirmAll() => Op("user confirm --all");

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
            command += $" --vault \"{vault.Uuid}\"";
        return Op<Document>(command);
    }

    public Group CreateGroup(string name, string description = "")
    {
        var command = $"group create \"{name}\"";
        if (description.Length > 0)
            command += $" --description \"{description}\"";
        return Op<Group>(command);
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
        if (template.PasswordRecipe != null && (template.Uuid == "001" || template.Uuid == "005")) // Login or Password
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
            command += $" --vault \"{vault.Uuid}\"";
        return Op<Item>(command);
    }

    public User CreateUser(string emailAddress, string name, string language = "en")
    {
        var command = $"user provision \"{emailAddress}\" \"{name}\"";
        if (language.Length > 0)
            command += $" --language \"{language}\"";
        return Op<User>(command);
    }

    public Vault CreateVault(string name, string description = "", bool allowAdminsToManage = true, VaultIcon icon = VaultIcon.Default)
    {
        var command = $"vault create \"{name}\"";
        if (!string.IsNullOrEmpty(description))
            command += $" --description \"{description}\"";
        if (!allowAdminsToManage)
            command += " --allow-admins-to-manage \"false\"";
        if (icon != VaultIcon.Default)
            command += $" --icon \"{GetIconName(icon)}\"";
        return Op<Vault>(command);
    }

    public void DeleteDocument(Document document) => DeleteDocument(document, null);

    public void DeleteDocument(Document document, Vault? vault)
    {
        var command = $"document delete \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        Op(command);
    }

    public void DeleteGroup(Group group) => Op($"group delete \"{group.Uuid}\"");

    public void DeleteItem(Item item) => DeleteItem(item, null);

    public void DeleteItem(Item item, Vault? vault)
    {
        var command = $"item delete \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        Op(command);
    }

    [Obsolete("The list events command has been removed in version 2 of the 1Password CLI.")]
    public void DeleteTrash(Vault vault) => throw new NotSupportedException("The list events command has been removed in version 2 of the 1Password CLI.");

    public void DeleteUser(User user) => Op($"user delete \"{user.Uuid}\"");

    public void DeleteVault(Vault vault) => Op($"vault delete \"{vault.Uuid}\"");

    public void EditGroup(Group group) => Op($"group edit \"{group.Uuid}\" --name \"{group.Name}\" --description \"{group.Description}\"");

    public void EditUser(User user, bool travelMode = false) => Op($"user edit \"{user.Uuid}\" --name \"{user.Name}\" --travelmode \"{(travelMode ? "on" : "off")}\"");

    public void EditVault(Vault vault)
    {
        var command = $"vault edit \"{vault.Uuid}\" --name \"{vault.Name}\" --description \"{vault.Description}\"";
        if (vault.Icon != VaultIcon.Default)
            command += $" --icon \"{GetIconName(vault.Icon)}\"";
        Op(command);
    }

    public void GetDocument(Item document, string path) => GetDocument(document, null, path);

    public void GetDocument(Item document, Vault? vault, string path)
    {
        var command = $"document get \"{document.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        command += $" --output \"{path}\"";
        Op(command);
    }

    public Group GetGroup(Group group) => Op<Group>($"group get \"{group.Uuid}\"");

    public Item GetItem(Item item) => GetItem(item, null);

    public Item GetItem(Item item, Vault? vault)
    {
        var command = $"item get \"{item.Uuid}\"";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        return Op<Item>(command);
    }

    public Template GetTemplate(Template template)
    {
        template.Details = Op<ItemDetails>($"item template gate \"{template.Name}\"");
        return template;
    }

    public User GetUser(User user) => Op<User>($"user get \"{user.Uuid}\"");

    public Vault GetVault(Vault vault) => Op<Vault>($"vault get \"{vault.Uuid}\"");

    public DocumentList ListDocuments(bool includeTrash = false) => ListDocuments(null, includeTrash);

    public DocumentList ListDocuments(Vault? vault, bool includeTrash = false)
    {
        var command = "document list";
        if (vault != null)
            command += $" --vault \"{vault.Uuid}\"";
        if (includeTrash)
            command += " --include-trash";
        return Op<DocumentList>(command);
    }

    public GroupList ListGroups() => Op<GroupList>("group list");

    public GroupList ListGroups(User user) => Op<GroupList>($"group list --user \"{user.Uuid}\"");

    public GroupList ListGroups(Vault vault) => Op<GroupList>($"group list --vault \"{vault.Uuid}\"");

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
            command += $" --vault \"{vault.Uuid}\"";
        if (template != null)
            command += $" --categories \"{template.Name}\"";
        if (!string.IsNullOrEmpty(tag))
            command += $" --tags \"{tag}\"";
        if (includeTrash)
            command += " --include-trash";
        return Op<ItemList>(command);
    }

    public TemplateList ListTemplates() => Op<TemplateList>("item template list");

    public UserList ListUsers() => Op<UserList>("user list");

    public UserList ListUsers(Group group) => Op<UserList>($"user list --group \"{group.Uuid}\"");

    public UserList ListUsers(Vault vault) => Op<UserList>($"vault user list \"{vault.Uuid}\"");

    public VaultList ListVaults() => Op<VaultList>("vault list");

    public VaultList ListVaults(User user) => Op<VaultList>($"vault list --user \"{user.Uuid}\"");

    public VaultList ListVaults(Group group) => Op<VaultList>($"vault list --group \"{group.Uuid}\"");

    public void ReactivateUser(User user) => Op($"user reactivate \"{user.Uuid}\"");

    public void RemoveGroup(Group group, Vault vault) => Op($"vault group revoke --vault \"{vault.Uuid}\" --group \"{group.Uuid}\"");

    public void RemoveUser(User user, Vault vault) => Op($"vault user revoke --vault \"{vault.Uuid}\" --user \"{user.Uuid}\"");

    public void RemoveUser(User user, Group group) => Op($"group user revoke --group \"{group.Uuid}\" --user \"{user.Uuid}\"");

    public void SuspendUser(User user, bool deauthorizeDevices = false, int deauthorizeDevicesDelay = 0)
    {
        var command = $"user suspend \"{user.Uuid}\"";
        if (deauthorizeDevices)
        {
            command += " --deauthorize-devices";
            if (deauthorizeDevicesDelay > 0)
                command += $" {deauthorizeDevicesDelay}";
        }
        Op(command);
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

    private static string GetIconName(VaultIcon vaultIcon)
    {
        var field = vaultIcon.GetType().GetField(vaultIcon.ToString());
        if (field is null)
            return "";

        var attributes = (IconAttribute[])field.GetCustomAttributes(typeof(IconAttribute), false);
        return attributes.Length > 0 ? attributes[0].Name : "";
    }

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
        return JsonSerializer.Deserialize<TResult>(Op(command, passAccount, passSession, returnError)) ?? throw new Exception("Could not deserialize the command result.");
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
        if (passAccount && string.IsNullOrWhiteSpace(_account))
            throw new Exception("Cannot execute command because account has not been set.");
        if (passSession && string.IsNullOrWhiteSpace(_session))
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