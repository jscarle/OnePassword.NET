using System.Diagnostics.CodeAnalysis;
using OnePassword.Accounts;
using OnePassword.Common;
using OnePassword.Documents;
using OnePassword.Groups;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Users;
using OnePassword.Vaults;

namespace OnePassword;

[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords")]
public interface IOnePasswordManager
{
    /// <summary>
    /// The version of the 1Password CLI executable.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Updates the 1Password CLI executable.
    /// </summary>
    /// <returns>
    /// Returns <see langword="true" /> when the 1Password CLI executable has been updated, <see langword="false" />
    /// otherwise.
    /// </returns>
    public bool Update();

    /// <summary>
    /// Gets the accounts.
    /// </summary>
    /// <returns>The list of accounts.</returns>
    public ImmutableList<Account> GetAccounts();

    /// <summary>
    /// Gets the account details.
    /// </summary>
    /// <param name="account">The account to retrieve.</param>
    /// <returns>The account details.</returns>
    public AccountDetails GetAccount(string account = "");

    /// <summary>
    /// Adds an account.
    /// </summary>
    /// <param name="address">The account address.</param>
    /// <param name="email">The account email.</param>
    /// <param name="secretKey">The account secret key.</param>
    /// <param name="password">The account password.</param>
    /// <param name="shorthand">The account shorthand.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void AddAccount(string address, string email, string secretKey, string password, string shorthand = "");

    /// <summary>
    /// Uses the account.
    /// </summary>
    /// <param name="account">The account to use.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void UseAccount(string account);

    /// <summary>
    /// Signs in to the account.
    /// </summary>
    /// <param name="password">The account password to use when manually signing in.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SignIn(string? password = null);

    /// <summary>
    /// Signs out of the account.
    /// </summary>
    /// <param name="all">When <see langword="true" />, signs out of all accounts.</param>
    public void SignOut(bool all = false);

    /// <summary>
    /// Forgets the account.
    /// </summary>
    /// <param name="all">When <see langword="true" />, forgets all accounts.</param>
    /// <returns>The list of accounts that were forgotten.</returns>
    public ImmutableList<string> ForgetAccount(bool all = false);

    /// <summary>
    /// Gets the vaults.
    /// </summary>
    /// <returns>The list of vaults.</returns>
    public ImmutableList<Vault> GetVaults();

    /// <summary>
    /// Gets a group's vaults.
    /// </summary>
    /// <param name="group">The group to retrieve vaults for.</param>
    /// <returns>The group's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IGroup group);

    /// <summary>
    /// Gets a user's vaults.
    /// </summary>
    /// <param name="user">The user to retrieve vaults for.</param>
    /// <returns>The user's vaults.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Vault> GetVaults(IUser user);

    /// <summary>
    /// Gets a vault.
    /// </summary>
    /// <param name="vault">The vault to retrieve.</param>
    /// <returns>The vault details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails GetVault(IVault vault);

    /// <summary>
    /// Creates a vault.
    /// </summary>
    /// <param name="name">The vault name.</param>
    /// <param name="description">The vault description.</param>
    /// <param name="icon">The vault icon.</param>
    /// <param name="allowAdminsToManage">When <see langword="true" />, allows administrators to manage the vault.</param>
    /// <returns>The created vault.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public VaultDetails CreateVault(string name, string? description = null, VaultIcon icon = VaultIcon.Default, bool? allowAdminsToManage = null);

    /// <summary>
    /// Edits a vault.
    /// </summary>
    /// <param name="vault">The vault to edit.</param>
    /// <param name="name">The vault's new name.</param>
    /// <param name="description">The vault's new description.</param>
    /// <param name="icon">The vault's new icon.</param>
    /// <param name="travelMode">
    /// When <see langword="true" />, enables travel mode on the vault. If enabled,
    /// <see langword="false" /> disables it.
    /// </param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditVault(IVault vault, string? name = null, string? description = null, VaultIcon icon = VaultIcon.Default, bool? travelMode = null);

    /// <summary>
    /// Deletes a vault.
    /// </summary>
    /// <param name="vault">The vault to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteVault(IVault vault);

    /// <summary>
    /// Grants a group permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="group">The group to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantPermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>
    /// Grants a user permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to grant permissions from.</param>
    /// <param name="user">The user to grant permissions to.</param>
    /// <param name="permissions">The permissions to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantPermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>
    /// Revokes a group's permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <param name="group">The group to revoke permissions to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokePermissions(IVault vault, IGroup group, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>
    /// Revokes a user's permissions to a vault.
    /// </summary>
    /// <param name="vault">The vault to revoke permissions from.</param>
    /// <param name="permissions">The permissions to revoke.</param>
    /// <param name="user">The user to revoke permissions to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokePermissions(IVault vault, IUser user, IReadOnlyCollection<VaultPermission> permissions);

    /// <summary>
    /// Gets a vault's items.
    /// </summary>
    /// <param name="vault">The vault that contains the items to retrieve.</param>
    /// <returns>The vault's items.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> GetItems(IVault vault);

    /// <summary>
    /// Searches for an item.
    /// <remarks>
    /// WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which
    /// may result in throttling.
    /// </remarks>
    /// </summary>
    /// <param name="vault">The vault that contains the items to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <param name="favorite">When <see langword="true" />, searches for favorites.</param>
    /// <param name="categories">The categories to search for.</param>
    /// <param name="tags">The tags to search for.</param>
    /// <returns>The items that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> SearchForItems(IVault? vault = null, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null,
        IReadOnlyCollection<string>? tags = null);

    /// <summary>
    /// Gets an item.
    /// </summary>
    /// <param name="item">The item to retrieve.</param>
    /// <param name="vault">The vault that contains the item to retrieve.</param>
    /// <returns>The item details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item GetItem(IItem item, IVault vault);

    /// <summary>
    /// Searches for an item.
    /// <remarks>
    /// WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which
    /// may result in throttling.
    /// </remarks>
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="vault">The vault that contains the item to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <returns>The item that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item SearchForItem(IItem item, IVault? vault = null, bool? includeArchive = null);

    /// <summary>
    /// Creates an item.
    /// </summary>
    /// <param name="template">The template from which to create the item.</param>
    /// <param name="vault">The vault in which to create the item.</param>
    /// <returns>The created item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item CreateItem(Template template, IVault vault);

    /// <summary>
    /// Edits an item.
    /// </summary>
    /// <param name="item">The item to edit.</param>
    /// <param name="vault">The vault that contains the item to edit.</param>
    /// <returns>The edited item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item EditItem(Item item, IVault vault);

    /// <summary>
    /// Archives an item.
    /// </summary>
    /// <param name="item">The item to archive.</param>
    /// <param name="vault">The vault that contains the item to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveItem(IItem item, IVault vault);

    /// <summary>
    /// Deletes an item.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    /// <param name="vault">The vault that contains the item to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteItem(IItem item, IVault vault);

    /// <summary>
    /// Gets the templates.
    /// </summary>
    /// <returns>The list of templates.</returns>
    public ImmutableList<TemplateInfo> GetTemplates();

    /// <summary>
    /// Gets a template.
    /// </summary>
    /// <param name="template">The template to retrieve.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(ITemplate template);

    /// <summary>
    /// Gets a template.
    /// </summary>
    /// <param name="name">The template name to retrieve.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(string name);

    /// <summary>
    /// Gets a template.
    /// </summary>
    /// <param name="category">The template category.</param>
    /// <returns>The template details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Template GetTemplate(Category category);

    /// <summary>
    /// Gets the users.
    /// </summary>
    /// <returns>The list of users.</returns>
    public ImmutableList<User> GetUsers();

    /// <summary>
    /// Gets a vault's users.
    /// </summary>
    /// <param name="vault">The vault for which to retrieve users.</param>
    /// <returns>The vault's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultUser> GetUsers(IVault vault);

    /// <summary>
    /// Gets a group's users.
    /// </summary>
    /// <param name="group">The group for which to retrieve users.</param>
    /// <returns>The group's users.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<GroupUser> GetUsers(IGroup group);

    /// <summary>
    /// Gets the user's details.
    /// </summary>
    /// <param name="user">The user to retrieve.</param>
    /// <returns>The user details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails GetUser(IUser user);

    /// <summary>
    /// Provisions a user.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="emailAddress">The user email address.</param>
    /// <param name="language">The user language.</param>
    /// <returns>The provisioned user.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public UserDetails ProvisionUser(string name, string emailAddress, Language language = Language.Default);

    /// <summary>
    /// Confirms a user.
    /// </summary>
    /// <param name="user">The user to confirm.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ConfirmUser(IUser user);

    /// <summary>
    /// Confirms all users.
    /// </summary>
    public void ConfirmAllUsers();

    /// <summary>
    /// Edits a user.
    /// </summary>
    /// <param name="user">The user to edit.</param>
    /// <param name="name">The user's new name.</param>
    /// <param name="travelMode">
    /// When <see langword="true" />, enables travel mode on the vault. If enabled,
    /// <see langword="false" /> disables it.
    /// </param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditUser(IUser user, string? name = null, bool? travelMode = null);

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteUser(IUser user);

    /// <summary>
    /// Suspends a user.
    /// </summary>
    /// <param name="user">The user to suspend.</param>
    /// <param name="deauthorizeDevicesDelay">
    /// The number of seconds to delay deauthorizing devices after the user has been
    /// suspended.
    /// </param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void SuspendUser(IUser user, int? deauthorizeDevicesDelay = null);

    /// <summary>
    /// Reactivates a user.
    /// </summary>
    /// <param name="user">The user to reactivate.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ReactivateUser(IUser user);

    /// <summary>
    /// Gets the groups.
    /// </summary>
    /// <returns>The list of groups.</returns>
    public ImmutableList<Group> GetGroups();

    /// <summary>
    /// Gets a vault's groups.
    /// </summary>
    /// <param name="vault">The vault for which to retrieve groups.</param>
    /// <returns>The vault's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<VaultGroup> GetGroups(IVault vault);

    /// <summary>
    /// Gets a user's groups.
    /// </summary>
    /// <param name="user">The user for which to retrieve groups.</param>
    /// <returns>The user's groups.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<UserGroup> GetGroups(IUser user);

    /// <summary>
    /// Gets the group's details.
    /// </summary>
    /// <param name="group">The group to retrieve.</param>
    /// <returns>The group details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails GetGroup(IGroup group);

    /// <summary>
    /// Create a group.
    /// </summary>
    /// <param name="name">The group name.</param>
    /// <param name="description">The group description.</param>
    /// <returns>The created group.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public GroupDetails CreateGroup(string name, string? description = null);

    /// <summary>
    /// Edits a group.
    /// </summary>
    /// <param name="group">The group to edit.</param>
    /// <param name="name">The group's new name.</param>
    /// <param name="description">The group's new description.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is nothing to edit.</exception>
    public void EditGroup(IGroup group, string? name = null, string? description = null);

    /// <summary>
    /// Deletes a group.
    /// </summary>
    /// <param name="group">The group to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteGroup(IGroup group);

    /// <summary>
    /// Grants a user access to a group.
    /// </summary>
    /// <param name="group">The group to grant access from.</param>
    /// <param name="user">The user to grant access to.</param>
    /// <param name="userRole">The user role to grant.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void GrantAccess(IGroup group, IUser user, UserRole userRole = UserRole.Member);

    /// <summary>
    /// Revokes a user's access to a group.
    /// </summary>
    /// <param name="group">The group to revoke access from.</param>
    /// <param name="user">The user to revoke access to.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void RevokeAccess(IGroup group, IUser user);

    /// <summary>
    /// Returns a list of all documents the account has read access to by default. Excludes items in the Archive by default.
    /// </summary>
    /// <param name="vault">Only list documents in this vault.</param>
    /// <param name="includeArchive">Include document items in the Archive. Can also be set using OP_INCLUDE_ARCHIVE environment variable.</param>
    /// <returns>The user's groups.</returns>
    public ImmutableList<Document> GetDocuments(
        string? vault = null,
        bool includeArchive = false);

    /// <summary>
    /// Create a document.
    /// When you create a document, a JSON object containing its ID is returned.
    /// The document is saved to the Private or Personal vault unless you specify another with the 'vault' option.
    /// </summary>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">Set the file's name.</param>
    /// <param name="title">Set the document item's title.</param>
    /// <param name="vault">Save the document in this vault. Default: Private.</param>
    /// <param name="tags">Set the tags to the specified (comma-separated) values.</param>
    /// <returns>The id of the newly created document.</returns>
    public CreateDocument CreateDocument(
        string filePath,
        string? fileName = null,
        string? title = null,
        string? vault = null,
        IReadOnlyCollection<string>? tags = null);

    /// <summary>
    /// Gets the content of a document, or download and save the document to disk.
    /// </summary>
    /// <param name="nameOrId">The name or Id of a document.</param>
    /// <param name="outFile">Save the document to the file path.</param>
    /// <param name="fileMode">Set filemode for the output file. (default 0600)</param>
    /// <param name="vault">Look for the document in this vault.</param>
    /// <param name="includeArchive">Include document items in the Archive. Can also be set using OP_INCLUDE_ARCHIVE environment variable.</param>
    /// <returns>The contents of the document if outFile not specified.</returns>
    public string GetDocument(
        string nameOrId,
        string? outFile = null,
        string? fileMode = null,
        string? vault = null,
        bool includeArchive = false);

    /// <summary>
    /// Edit a document.
    /// Replaces the file contents of a Document item with the provided file.
    /// </summary>
    /// <param name="nameOrId">The name or Id of a document.</param>
    /// <param name="filePath">The path to the file to upload.</param>
    /// <param name="fileName">Set the file's name.</param>
    /// <param name="title">Set the document item's title.</param>
    /// <param name="vault">Save the document in this vault. Default: Private.</param>
    /// <param name="tags">Set the tags to the specified (comma-separated) values.</param>
    public void EditDocument(
        string nameOrId,
        string filePath,
        string? fileName = null,
        string? title = null,
        string? vault = null,
        IReadOnlyCollection<string>? tags = null);
}