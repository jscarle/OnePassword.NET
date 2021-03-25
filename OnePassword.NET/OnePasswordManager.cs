using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using OnePassword.Documents;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword
{
    public class OnePasswordManager
    {
        private readonly string _opPath;
        private readonly bool _verbose;
        private string _shorthand;
        private string _sessionId;

        public OnePasswordManager(string path = "", string executable = "op.exe", bool verbose = false)
        {
            _opPath = !string.IsNullOrEmpty(path) ? Path.Combine(path, executable) : Path.Combine(Directory.GetCurrentDirectory(), executable);
            if (!File.Exists(_opPath))
                throw new Exception($"The 1Password CLI executable ({executable}) was not found in folder \"{Path.GetDirectoryName(_opPath)}\".");

            _verbose = verbose;

            _sessionId = string.Empty;
        }

        public void AddGroup(Group group, Vault vault) => Op($"add group \"{group.Uuid}\" \"{vault.Uuid}\"");

        public void AddUser(User user, Vault vault) => Op($"add user \"{user.Uuid}\" \"{vault.Uuid}\"");

        public void AddUser(User user, Group group, UserRole userRole = UserRole.Member)
        {
            string command = $"add user \"{user.Uuid}\" \"{group.Uuid}\"";
            if (userRole == UserRole.Manager)
                command += " --role manager";
            Op(command);
        }

        public void ConfirmUser(User user) => Op($"confirm user \"{user.Uuid}\"");

        public void ConfirmAll() => Op($"confirm user --all");

        public Document CreateDocument(Template template, string path) => CreateDocument(template, path, null);

        public Document CreateDocument(Template template, string path, Vault vault)
        {
            if (template.Uuid != "006") // Document
                throw new ArgumentException("Cannot create an Item using this method. Use CreateItem instead.");

            string command = $"create document \"{path}\"";
            if (!string.IsNullOrEmpty(template.Title))
                command += $" --title \"{template.Title}\"";
            else
                command += $" --title \"{template.Name}\"";
            if (!string.IsNullOrEmpty(template.Filename))
                command += $" --filename \"{template.Filename}\"";
            if (template.Tags.Count > 0)
                command += $" --tags \"{string.Join(",", template.Tags.ToArray())}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            return JsonConvert.DeserializeObject<Document>(Op(command));
        }

        public Group CreateGroup(string name, string description = "")
        {
            string command = $"create group \"{name}\"";
            if (description.Length > 0)
                command += $" --description \"{description}\"";
            return JsonConvert.DeserializeObject<Group>(Op(command));
        }

        public Item CreateItem(Template template) => CreateItem(template, null);

        public Item CreateItem(Template template, Vault vault)
        {
            if (template.Uuid == "006") // Document
                throw new ArgumentException("Cannot create a Document using this method. Use CreateDocument instead.");

            string command = $"create item \"{template.Name}\" \"{template.Details.ToBase64()}\"";
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
            return JsonConvert.DeserializeObject<Item>(Op(command));
        }

        public User CreateUser(string emailAddress, string name, string language = "en")
        {
            string command = $"create user \"{emailAddress}\" \"{name}\"";
            if (language.Length > 0)
                command += $" --language \"{language}\"";
            return JsonConvert.DeserializeObject<User>(Op(command));
        }

        public Vault CreateVault(string name, string description = "", bool allowAdminsToManage = true)
        {
            string command = $"create vault \"{name}\"";
            if (!string.IsNullOrEmpty(description))
                command += $" --description \"{description}\"";
            if (!allowAdminsToManage)
                command += $" --allow-admins-to-manage \"false\"";
            return JsonConvert.DeserializeObject<Vault>(Op(command));
        }

        public void DeleteDocument(Document document) => Op($"delete document \"{document.Uuid}\"");

        public void DeleteGroup(Group group) => Op($"delete group \"{group.Uuid}\"");

        public void DeleteItem(Item item) => Op($"delete item \"{item.Uuid}\"");

        public void DeleteTrash(Vault vault) => Op($"delete trash \"{vault.Uuid}\"");

        public void DeleteUser(User user) => Op($"delete user \"{user.Uuid}\"");

        public void DeleteVault(Vault vault) => Op($"delete vault \"{vault.Uuid}\"");

        public void EditGroup(Group group) => Op($"edit group \"{group.Uuid}\" --name \"{group.Name}\" --description \"{group.Description}\"");

        public void EditUser(User user, bool travelMode = false) => Op($"edit user \"{user.Uuid}\" --name \"{user.Name}\" --travelmode \"{(travelMode ? "on" : "off")}\"");

        public void EditVault(Vault vault) => Op($"edit vault \"{vault.Uuid}\" --name \"{vault.Name}\"");

        public void Forget(string domain) => Op($"forget {domain}");

        public Account GetAccount() => JsonConvert.DeserializeObject<Account>(Op("get account"));

        public void GetDocument(Item document, string path) => Op($"get document \"{document.Uuid}\" --output \"{path}\"");

        public Group GetGroup(Group group) => JsonConvert.DeserializeObject<Group>(Op($"get group \"{group.Uuid}\""));

        public Item GetItem(Item item) => JsonConvert.DeserializeObject<Item>(Op($"get item \"{item.Uuid}\""));

        public Template GetTemplate(Template template)
        {
            template.Details = JsonConvert.DeserializeObject<ItemDetails>(Op($"get template \"{template.Name}\""));
            return template;
        } 

        public User GetUser(User user) => JsonConvert.DeserializeObject<User>(Op($"get user \"{user.Uuid}\""));

        public Vault GetVault(Vault vault) => JsonConvert.DeserializeObject<Vault>(Op($"get vault \"{vault.Uuid}\""));

        public DocumentList ListDocuments(bool includeTrash = false) => ListDocuments(null, includeTrash);

        public DocumentList ListDocuments(Vault vault, bool includeTrash = false)
        {
            string command = "list documents";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            if (includeTrash)
                command += " --include-trash";
            return JsonConvert.DeserializeObject<DocumentList>(Op(command));
        }

        public List<Event> ListEvents() => JsonConvert.DeserializeObject<List<Event>>(Op("list events"));

        public List<Event> ListEvents(int eventId, bool older = false)
        {
            string command = $"list events --eventid {eventId}";
            if (older)
                command += " --older";
            return JsonConvert.DeserializeObject<List<Event>>(Op(command));
        }

        public GroupList ListGroups() => JsonConvert.DeserializeObject<GroupList>(Op("list groups"));

        public GroupList ListGroups(User user) => JsonConvert.DeserializeObject<GroupList>(Op($"list groups --user \"{user.Uuid}\""));

        public GroupList ListGroups(Vault vault) => JsonConvert.DeserializeObject<GroupList>(Op($"list groups --vault \"{vault.Uuid}\""));

        public ItemList ListItems(bool includeTrash = false) => ListItems(null, null, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, bool includeTrash = false) => ListItems(vault, null, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, Template template, bool includeTrash = false) => ListItems(vault, template, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, string tag, bool includeTrash = false) => ListItems(vault, null, tag, includeTrash);

        public ItemList ListItems(Template template, bool includeTrash = false) => ListItems(null, template, string.Empty, includeTrash);

        public ItemList ListItems(Template template, string tag, bool includeTrash = false) => ListItems(null, template, tag, includeTrash);

        public ItemList ListItems(string tag, bool includeTrash = false) => ListItems(null, null, tag, includeTrash);

        public ItemList ListItems(Vault vault, Template template, string tag, bool includeTrash = false)
        {
            string command = "list items";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            if (template != null)
                command += $" --categories \"{template.Name}\"";
            if (!string.IsNullOrEmpty(tag))
                command += $" --tags \"{tag}\"";
            if (includeTrash)
                command += " --include-trash";
            return JsonConvert.DeserializeObject<ItemList>(Op(command));
        }

        public TemplateList ListTemplates() => JsonConvert.DeserializeObject<TemplateList>(Op("list templates"));

        public UserList ListUsers() => JsonConvert.DeserializeObject<UserList>(Op("list users"));

        public UserList ListUsers(Group group) => JsonConvert.DeserializeObject<UserList>(Op($"list users --group \"{group.Uuid}\""));

        public UserList ListUsers(Vault vault) => JsonConvert.DeserializeObject<UserList>(Op($"list users --vault \"{vault.Uuid}\""));

        public VaultList ListVaults() => JsonConvert.DeserializeObject<VaultList>(Op("list vaults"));

        public VaultList ListVaults(User user) => JsonConvert.DeserializeObject<VaultList>(Op($"list vaults --user \"{user.Uuid}\""));

        public VaultList ListVaults(Group group) => JsonConvert.DeserializeObject<VaultList>(Op($"list vaults --group \"{group.Uuid}\""));

        public void ReactivateUser(User user) => Op($"reactivate \"{user.Uuid}\"");

        public void RemoveGroup(Group group, Vault vault) => Op($"remove group \"{group.Uuid}\" \"{vault.Uuid}\"");

        public void RemoveUser(User user, Vault vault) => Op($"remove user \"{user.Uuid}\" \"{vault.Uuid}\"");

        public void RemoveUser(User user, Group group) => Op($"remove user \"{user.Uuid}\" \"{group.Uuid}\"");

        public void SignIn(string domain, string email, string secretKey, string password, string shorthand = "")
        {
            Regex opDeviceRegex = new Regex("OP_DEVICE=(?<UUID>[a-z0-9]+)");

            string command = $"signin {domain} {email} {secretKey} --raw";
            if (!string.IsNullOrEmpty(shorthand))
                command += $" --shorthand {shorthand}";

            string result = Op(command, password, true);
            if (result.Contains("No saved device ID."))
            {
                string deviceUuid = opDeviceRegex.Match(result).Groups["UUID"].Value;
                Environment.SetEnvironmentVariable("OP_DEVICE", deviceUuid);
                result = Op(command, password);
            }

            if (result.StartsWith("[ERROR]"))
                throw new Exception(result.Length > 28 ? result.Substring(28).Trim() : result);

            _shorthand = shorthand;
            _sessionId = result.Trim();
        }

        public void SignOut(bool forget = false)
        {
            string command = "signout";
            if (forget)
                command += " --forget";
            Op(command);
        }

        public void SuspendUser(User user) => Op($"suspend \"{user.Uuid}\"");

        private static string GetStandardError(Process process)
        {
            StringBuilder error = new StringBuilder();
            while (process.StandardError.Peek() > -1)
                error.Append((char)process.StandardError.Read());
            return error.ToString();
        }

        private static string GetStandardOutput(Process process)
        {
            StringBuilder error = new StringBuilder();
            while (process.StandardOutput.Peek() > -1)
                error.Append((char)process.StandardOutput.Read());
            return error.ToString();
        }

        private string Op(string command, string input = "", bool returnError = false)
        {
            string arguments = command;
            if (!string.IsNullOrEmpty(_sessionId))
                arguments += $" --session {_sessionId}";
            if (!string.IsNullOrEmpty(_shorthand))
                arguments += $" --shorthand {_shorthand}";

            if (_verbose)
                Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

            ProcessStartInfo processStartInfo = new ProcessStartInfo(_opPath, $"{arguments}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            Process process = Process.Start(processStartInfo);

            if (process is null)
                throw new Exception($"Could not start process for {_opPath}.");

            if (input.Length > 0)
                process.StandardInput.WriteLine(input);

            string output = GetStandardOutput(process);
            if (_verbose)
                Console.WriteLine(output);

            string error = GetStandardError(process);
            if (_verbose)
                Console.WriteLine(error);

            if (error.StartsWith("[ERROR]"))
            {
                if (returnError)
                    return error;

                throw new Exception(error.Length > 28 ? error.Substring(28).Trim() : error);
            }

            return output;
        }
    }
}
