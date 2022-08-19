using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private readonly TimeSpan _totpProcessStartupTimeout = TimeSpan.FromMilliseconds(1000);
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

        public void AddGroup(Group group, Vault vault, string permission = "allow_editing,allow_viewing,allow_managing") => Op($"vault group grant --vault \"{vault.Uuid}\" --group \"{group.Uuid}\" --permission \"{permission}\"");

        public void AddUser(User user, Vault vault, string permission = "allow_editing,allow_viewing,allow_managing") => Op($"vault user grant --vault \"{vault.Uuid}\" --user \"{user.Uuid}\" --permission \"{permission}\"");

        public void AddUser(User user, Group group, UserRole userRole = UserRole.Member)
        {
            string command = $"group user grant --group \"{group.Uuid}\" --user \"{user.Uuid}\"";
            if (userRole == UserRole.Manager)
                command += " --role manager";
            Op(command);
        }

        public void ArchiveDocument(Document document) => ArchiveDocument(document, null);

        public void ArchiveDocument(Document document, Vault vault)
        {
            string command = $"document delete \"{document.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            command += $" --archive";
            Op(command);
        }

        public void ArchiveItem(Item item) => ArchiveItem(item, null);

        public void ArchiveItem(Item item, Vault vault)
        {
            string command = $"item delete \"{item.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            command += $" --archive";
            Op(command);
        }

        public void ConfirmUser(User user) => Op($"user confirm \"{user.Uuid}\"");

        public void ConfirmAll() => Op($"user confirm --all");

        public Document CreateDocument(Template template, string path) => CreateDocument(template, null, path);

        public Document CreateDocument(Template template, Vault vault, string path)
        {
            if (template.Uuid != "006") // Document
                throw new ArgumentException("Cannot create an Item using this method. Use CreateItem instead.");

            string command = $"document create \"{path}\"";
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
            return JsonConvert.DeserializeObject<Document>(Op(command));
        }

        public Group CreateGroup(string name, string description = "")
        {
            string command = $"group create \"{name}\"";
            if (description.Length > 0)
                command += $" --description \"{description}\"";
            return JsonConvert.DeserializeObject<Group>(Op(command));
        }

        public Item CreateItem(Template template) => CreateItem(template, null);

        public Item CreateItem(Template template, Vault vault)
        {
            if (template.Uuid == "006") // Document
                throw new ArgumentException("Cannot create a Document using this method. Use CreateDocument instead.");

            string command = $"item create \"{template.Name}\" \"{template.Details.ToBase64()}\"";
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
            string command = $"user provision \"{emailAddress}\" \"{name}\"";
            if (language.Length > 0)
                command += $" --language \"{language}\"";
            return JsonConvert.DeserializeObject<User>(Op(command));
        }

        public Vault CreateVault(string name, string description = "", bool allowAdminsToManage = true, VaultIcon icon = VaultIcon.Default)
        {
            string command = $"vault create \"{name}\"";
            if (!string.IsNullOrEmpty(description))
                command += $" --description \"{description}\"";
            if (!allowAdminsToManage)
                command += $" --allow-admins-to-manage \"false\"";
            if (icon != VaultIcon.Default)
                command += $" --icon \"{GetIconName(icon)}\"";
            return JsonConvert.DeserializeObject<Vault>(Op(command));
        }

        public void DeleteDocument(Document document) => DeleteDocument(document, null);

        public void DeleteDocument(Document document, Vault vault)
        {
            string command = $"document delete \"{document.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            Op(command);
        }

        public void DeleteGroup(Group group) => Op($"group delete \"{group.Uuid}\"");

        public void DeleteItem(Item item) => DeleteItem(item, null);

        public void DeleteItem(Item item, Vault vault)
        {
            string command = $"item delete \"{item.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            Op(command);
        }

        [Obsolete]
        public void DeleteTrash(Vault vault) => throw new NotSupportedException("The list events command has been removed in version 2 of the 1Password CLI.");

        public void DeleteUser(User user) => Op($"user delete \"{user.Uuid}\"");

        public void DeleteVault(Vault vault) => Op($"vault delete \"{vault.Uuid}\"");

        public void EditGroup(Group group) => Op($"group edit \"{group.Uuid}\" --name \"{group.Name}\" --description \"{group.Description}\"");

        public void EditUser(User user, bool travelMode = false) => Op($"user edit \"{user.Uuid}\" --name \"{user.Name}\" --travelmode \"{(travelMode ? "on" : "off")}\"");

        public void EditVault(Vault vault)
        {
            string command = $"vault edit \"{vault.Uuid}\" --name \"{vault.Name}\" --description \"{vault.Description}\"";
            if (vault.Icon != VaultIcon.Default)
                command += $" --icon \"{GetIconName(vault.Icon)}\"";
            Op(command);
        }

        public void Forget(string domain) => Op($"account forget {domain}");

        public Account GetAccount() => JsonConvert.DeserializeObject<Account>(Op("account get"));

        public void GetDocument(Item document, string path) => GetDocument(document, null, path);

        public void GetDocument(Item document, Vault vault, string path)
        {
            string command = $"document get \"{document.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            command += $" --output \"{path}\"";
            Op(command);
        }

        public Group GetGroup(Group group) => JsonConvert.DeserializeObject<Group>(Op($"group get \"{group.Uuid}\""));

        public Item GetItem(Item item) => GetItem(item, null);

        public Item GetItem(Item item, Vault vault)
        {
            string command = $"item get \"{item.Uuid}\"";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            return JsonConvert.DeserializeObject<Item>(Op(command));
        }

        public Template GetTemplate(Template template)
        {
            template.Details = JsonConvert.DeserializeObject<ItemDetails>(Op($"item template gate \"{template.Name}\""));
            return template;
        }

        public User GetUser(User user) => JsonConvert.DeserializeObject<User>(Op($"user get \"{user.Uuid}\""));

        public Vault GetVault(Vault vault) => JsonConvert.DeserializeObject<Vault>(Op($"vault get \"{vault.Uuid}\""));

        public DocumentList ListDocuments(bool includeTrash = false) => ListDocuments(null, includeTrash);

        public DocumentList ListDocuments(Vault vault, bool includeTrash = false)
        {
            string command = "document list";
            if (vault != null)
                command += $" --vault \"{vault.Uuid}\"";
            if (includeTrash)
                command += " --include-trash";
            return JsonConvert.DeserializeObject<DocumentList>(Op(command));
        }

        [Obsolete]
        public List<Event> ListEvents() => throw new NotSupportedException("The list events command has been removed in version 2 of the 1Password CLI.");

        [Obsolete]
        public List<Event> ListEvents(int eventId, bool older = false) => throw new NotSupportedException("The list events command has been removed in version 2 of the 1Password CLI.");

        public GroupList ListGroups() => JsonConvert.DeserializeObject<GroupList>(Op("group list"));

        public GroupList ListGroups(User user) => JsonConvert.DeserializeObject<GroupList>(Op($"group list --user \"{user.Uuid}\""));

        public GroupList ListGroups(Vault vault) => JsonConvert.DeserializeObject<GroupList>(Op($"group list --vault \"{vault.Uuid}\""));

        public ItemList ListItems(bool includeTrash = false) => ListItems(null, null, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, bool includeTrash = false) => ListItems(vault, null, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, Template template, bool includeTrash = false) => ListItems(vault, template, string.Empty, includeTrash);

        public ItemList ListItems(Vault vault, string tag, bool includeTrash = false) => ListItems(vault, null, tag, includeTrash);

        public ItemList ListItems(Template template, bool includeTrash = false) => ListItems(null, template, string.Empty, includeTrash);

        public ItemList ListItems(Template template, string tag, bool includeTrash = false) => ListItems(null, template, tag, includeTrash);

        public ItemList ListItems(string tag, bool includeTrash = false) => ListItems(null, null, tag, includeTrash);

        public ItemList ListItems(Vault vault, Template template, string tag, bool includeTrash = false)
        {
            string command = "item list";
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

        public TemplateList ListTemplates() => JsonConvert.DeserializeObject<TemplateList>(Op("item template list"));

        public UserList ListUsers() => JsonConvert.DeserializeObject<UserList>(Op("user list"));

        public UserList ListUsers(Group group) => JsonConvert.DeserializeObject<UserList>(Op($"user list --group \"{group.Uuid}\""));

        public UserList ListUsers(Vault vault) => JsonConvert.DeserializeObject<UserList>(Op($"vault user list \"{vault.Uuid}\""));

        public VaultList ListVaults() => JsonConvert.DeserializeObject<VaultList>(Op("vault list"));

        public VaultList ListVaults(User user) => JsonConvert.DeserializeObject<VaultList>(Op($"vault list --user \"{user.Uuid}\""));

        public VaultList ListVaults(Group group) => JsonConvert.DeserializeObject<VaultList>(Op($"vault list --group \"{group.Uuid}\""));

        public void ReactivateUser(User user) => Op($"user reactivate \"{user.Uuid}\"");

        public void RemoveGroup(Group group, Vault vault) => Op($"vault group revoke --vault \"{vault.Uuid}\" --group \"{group.Uuid}\"");

        public void RemoveUser(User user, Vault vault) => Op($"vault user revoke --vault \"{vault.Uuid}\" --user \"{user.Uuid}\"");

        public void RemoveUser(User user, Group group) => Op($"group user revoke --group \"{group.Uuid}\" --user \"{user.Uuid}\"");

        public void SignIn(string domain, string email, string secretKey, string password, string shorthand = "")
        {
            SignIn(domain, email, secretKey, password, "", shorthand);
        }

        public void SignInTotp(string domain, string email, string secretKey, string password, string totp, string shorthand = "")
        {
            SignIn(domain, email, secretKey, password, totp, shorthand);
        }

        private void SignIn(string domain, string email, string secretKey, string password, string totp, string shorthand)
        {
            string command = $"account add --address {domain} --email {email} --secret-key {secretKey}";
            if (!string.IsNullOrEmpty(shorthand))
                command += $" --shorthand {shorthand}";

            string result = Op(command, new string[] { password, totp }, true);
            if (result.Contains("No saved device ID."))
            {
                Regex opDeviceRegex = new Regex("OP_DEVICE=(?<UUID>[a-z0-9]+)");
                string deviceUuid = opDeviceRegex.Match(result).Groups["UUID"].Value;
                Environment.SetEnvironmentVariable("OP_DEVICE", deviceUuid);

                result = Op(command, new string[] { password, totp });
            }

            if (result.StartsWith("[ERROR]"))
                throw new Exception(result.Length > 28 ? result.Substring(28).Trim() : result);

            command = $"signin --raw";
            result = Op(command, new string[] { password, totp }, true);
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

        public void SuspendUser(User user, bool deauthorizeDevices = false, int deauthorizeDevicesDelay = 0)
        {
            string command = $"user suspend \"{user.Uuid}\"";
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
            bool updated = false;

            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);

            string command = $"update --directory \"{tempDirectory}\"";
            string result = Op(command);

            if (Regex.Match(result, @"Version ([^\s]+) is now available\.").Success)
                foreach (string file in Directory.GetFiles(tempDirectory, "*.zip"))
                    using (ZipArchive zipArchive = ZipFile.Open(file, ZipArchiveMode.Read))
                    {
                        zipArchive.GetEntry("op.exe")?.ExtractToFile(_opPath, true);
                        updated = true;
                    }

            Directory.Delete(tempDirectory, true);

            return updated;
        }
#endif

        private static string GetIconName(VaultIcon vaultIcon)
        {
            IconAttribute[] attributes = (IconAttribute[])vaultIcon.GetType().GetField(vaultIcon.ToString()).GetCustomAttributes(typeof(IconAttribute), false);
            return attributes.Length > 0 ? attributes[0].Name : string.Empty;
        }

        private static string GetStandardError(Process process)
        {
            StringBuilder error = new StringBuilder();
            while (process.StandardError.Peek() > -1)
                error.Append((char)process.StandardError.Read());
            return error.ToString();
        }

        private static string GetStandardOutput(Process process)
        {
            StringBuilder output = new StringBuilder();
            while (process.StandardOutput.Peek() > -1)
                output.Append((char)process.StandardOutput.Read());
            return output.ToString();
        }

        private string Op(string command, string input = "", bool returnError = false)
        {
            return Op(command, new string[] { input });
        }

        private string Op(string command, string[] input, bool returnError = false)
        {

            string arguments = command;
            if (!command.StartsWith("signout") && !string.IsNullOrEmpty(_sessionId))
                arguments += $" --session {_sessionId}";
            if (!string.IsNullOrEmpty(_shorthand))
                arguments += $" --account {_shorthand}";

            if (_verbose)
                Console.WriteLine($"{Path.GetDirectoryName(_opPath)}>op {arguments}");

            ProcessStartInfo processStartInfo = new ProcessStartInfo(_opPath, $"{arguments} --format json --no-color")
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

            if (input != null && input.Length > 0)
            {
                if (input.Length > 1)
                    Thread.Sleep(_totpProcessStartupTimeout);

                foreach (var inputLine in input)
                {
                    process.StandardInput.WriteLine(inputLine);
                    process.StandardInput.Flush();
                }
            }

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
