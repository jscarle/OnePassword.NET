using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using OnePassword.Items;
using OnePassword.Users;

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

        public void AddGroup(Group group, Vault vault) => op($"add group \"{group.Uuid}\" \"{vault.Uuid}\"");

        public void AddUser(User user, Vault vault) => op($"add user \"{user.Uuid}\" \"{vault.Uuid}\"");

        public void AddUser(User user, Group group, UserRole userRole = UserRole.Member)
        {
            string command = $"add user \"{user.Uuid}\" \"{group.Uuid}\"";
            if (userRole == UserRole.Manager)
                command += " --role manager";
            op(command);
        }

        public void ConfirmUser(User user) => op($"confirm user \"{user.Uuid}\"");

        public void ConfirmAll() => op($"confirm user --all");

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
            return JsonConvert.DeserializeObject<Document>(op(command));
        }

        public Group CreateGroup(string name, string description = "")
        {
            string command = $"create group \"{name}\"";
            if (description.Length > 0)
                command += $" --description \"{description}\"";
            return JsonConvert.DeserializeObject<Group>(op(command));
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
            return JsonConvert.DeserializeObject<Item>(op(command));
        }

        public User CreateUser(string emailAddress, string name, string language = "en")
        {
            string command = $"create user \"{emailAddress}\" \"{name}\"";
            if (language.Length > 0)
                command += $" --language \"{language}\"";
            return JsonConvert.DeserializeObject<User>(op(command));
        }

        public Vault CreateVault(string name, string description = "", bool allowAdminsToManage = true)
        {
            string command = $"create vault \"{name}\"";
            if (!string.IsNullOrEmpty(description))
                command += $" --description \"{description}\"";
            if (!allowAdminsToManage)
                command += $" --allow-admins-to-manage \"false\"";
            return JsonConvert.DeserializeObject<Vault>(op(command));
        }

        public void DeleteDocument(Document document) => op($"delete document \"{document.Uuid}\"");

        public void DeleteGroup(Group group) => op($"delete group \"{group.Uuid}\"");

        public void DeleteItem(Item item) => op($"delete item \"{item.Uuid}\"");

        public void DeleteTrash(Vault vault) => op($"delete trash \"{vault.Uuid}\"");

        public void DeleteUser(User user) => op($"delete user \"{user.Uuid}\"");

        public void DeleteVault(Vault vault) => op($"delete vault \"{vault.Uuid}\"");

        public void EditGroup(Group group) => op($"edit group \"{group.Uuid}\" --name \"{group.Name}\" --description \"{group.Description}\"");

        public void EditUser(User user, bool travelMode = false) => op($"edit user \"{user.Uuid}\" --name \"{user.Name}\" --travelmode \"{(travelMode ? "on" : "off")}\"");

        public void EditVault(Vault vault) => op($"edit vault \"{vault.Uuid}\" --name \"{vault.Name}\"");

        public void Forget(string domain) => op($"forget {domain}");

        public Account GetAccount() => JsonConvert.DeserializeObject<Account>(op("get account"));

        public void GetDocument(Item document, string path) => op($"get document \"{document.Uuid}\" --output \"{path}\"");

        public Group GetGroup(Group group) => JsonConvert.DeserializeObject<Group>(op($"get group \"{group.Uuid}\""));

        public Item GetItem(Item item) => JsonConvert.DeserializeObject<Item>(op($"get item \"{item.Uuid}\""));

        public Template GetTemplate(Template template)
        {
            template.Details = JsonConvert.DeserializeObject<ItemDetails>(op($"get template \"{template.Name}\""));
            return template;
        } 

        public User GetUser(User user) => JsonConvert.DeserializeObject<User>(op($"get user \"{user.Uuid}\""));

        public Vault GetVault(Vault vault) => JsonConvert.DeserializeObject<Vault>(op($"get vault \"{vault.Uuid}\""));

        public List<Item> ListDocuments(bool includeTrash = false) => ListDocuments(null, includeTrash);

        public List<Item> ListDocuments(Vault vault, bool includeTrash = false)
        {
            string command = "list documents";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            if (includeTrash)
                command += " --include-trash";
            return JsonConvert.DeserializeObject<List<Item>>(op(command));
        }

        public List<Event> ListEvents() => JsonConvert.DeserializeObject<List<Event>>(op("list events"));

        public List<Event> ListEvents(int eventID, bool older = false)
        {
            string command = $"list events --eventid {eventID}";
            if (older)
                command += " --older";
            return JsonConvert.DeserializeObject<List<Event>>(op(command));
        }

        public List<Group> ListGroups() => JsonConvert.DeserializeObject<List<Group>>(op("list groups"));

        public List<Group> ListGroups(User user) => JsonConvert.DeserializeObject<List<Group>>(op($"list groups --user \"{user.Uuid}\""));

        public List<Group> ListGroups(Vault vault) => JsonConvert.DeserializeObject<List<Group>>(op($"list groups --vault \"{vault.Uuid}\""));

        public List<Item> ListItems(bool includeTrash = false) => ListItems(null, null, string.Empty, includeTrash);

        public List<Item> ListItems(Vault vault, bool includeTrash = false) => ListItems(vault, null, string.Empty, includeTrash);

        public List<Item> ListItems(Vault vault, Template template, bool includeTrash = false) => ListItems(vault, template, string.Empty, includeTrash);

        public List<Item> ListItems(Vault vault, string tag, bool includeTrash = false) => ListItems(vault, null, tag, includeTrash);

        public List<Item> ListItems(Template template, bool includeTrash = false) => ListItems(null, template, string.Empty, includeTrash);

        public List<Item> ListItems(Template template, string tag, bool includeTrash = false) => ListItems(null, template, tag, includeTrash);

        public List<Item> ListItems(string tag, bool includeTrash = false) => ListItems(null, null, tag, includeTrash);

        public List<Item> ListItems(Vault vault, Template template, string tag, bool includeTrash = false)
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
            return JsonConvert.DeserializeObject<List<Item>>(op(command));
        }

        public List<Template> ListTemplates() => JsonConvert.DeserializeObject<List<Template>>(op("list templates"));

        public List<User> ListUsers() => JsonConvert.DeserializeObject<List<User>>(op("list users"));

        public List<User> ListUsers(Group group) => JsonConvert.DeserializeObject<List<User>>(op($"list users --group \"{group.Uuid}\""));

        public List<User> ListUsers(Vault vault) => JsonConvert.DeserializeObject<List<User>>(op($"list users --vault \"{vault.Uuid}\""));

        public List<Vault> ListVaults() => JsonConvert.DeserializeObject<List<Vault>>(op("list vaults"));

        public List<Vault> ListVaults(User user) => JsonConvert.DeserializeObject<List<Vault>>(op($"list vaults --user \"{user.Uuid}\""));

        public List<Vault> ListVaults(Group group) => JsonConvert.DeserializeObject<List<Vault>>(op($"list vaults --group \"{group.Uuid}\""));

        public void ReactivateUser(User user) => op($"reactivate \"{user.Uuid}\"");

        public void RemoveGroup(Group group, Vault vault) => op($"remove group \"{group.Uuid}\" \"{vault.Uuid}\"");

        public void RemoveUser(User user, Vault vault) => op($"remove user \"{user.Uuid}\" \"{vault.Uuid}\"");

        public void RemoveUser(User user, Group group) => op($"remove user \"{user.Uuid}\" \"{group.Uuid}\"");

        public void SignIn(string domain, string email, string secretKey, string password, string shorthand = "")
        {
            Regex OpDeviceRegex = new Regex("OP_DEVICE=(?<UUID>[a-z0-9]+)");

            string command = $"signin {domain} {email} {secretKey} --raw";
            if (!string.IsNullOrEmpty(shorthand))
                command += $" --shorthand {shorthand}";

            string result = op(command, password, true);
            if (result.Contains("No saved device ID."))
            {
                string deviceUUID = OpDeviceRegex.Match(result).Groups["UUID"].Value;
                Environment.SetEnvironmentVariable("OP_DEVICE", deviceUUID);
                result = op(command, password);
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
            op(command);
        }

        public void SuspendUser(User user) => op($"suspend \"{user.Uuid}\"");

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

        private string op(string command, string input = "", bool returnError = false)
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
