using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;

namespace OnePassword;

public partial interface IOnePasswordManager
{
    /// <summary>Gets a vault's items.</summary>
    /// <param name="vault">The vault that contains the items to retrieve.</param>
    /// <returns>The vault's items.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> GetItems(IVault vault);

    /// <summary>Gets a vault's items.</summary>
    /// <param name="vaultId">The ID of the vault that contains the items to retrieve.</param>
    /// <returns>The vault's items.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> GetItems(string vaultId);

    /// <summary>Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="vault">The vault that contains the items to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <param name="favorite">When <see langword="true" />, searches for favorites.</param>
    /// <param name="categories">The categories to search for.</param>
    /// <param name="tags">The tags to search for.</param>
    /// <returns>The items that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> SearchForItems(IVault vault, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null, IReadOnlyCollection<string>? tags = null);

    /// <summary>Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="vaultId">The ID of the vault that contains the items to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <param name="favorite">When <see langword="true" />, searches for favorites.</param>
    /// <param name="categories">The categories to search for.</param>
    /// <param name="tags">The tags to search for.</param>
    /// <returns>The items that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> SearchForItems(string? vaultId = null, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null,
        IReadOnlyCollection<string>? tags = null);

    /// <summary>Gets an item.</summary>
    /// <param name="item">The item to retrieve.</param>
    /// <param name="vault">The vault that contains the item to retrieve.</param>
    /// <returns>The item details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item GetItem(IItem item, IVault vault);

    /// <summary>Gets an item.</summary>
    /// <param name="itemId">The ID of the item to retrieve.</param>
    /// <param name="vaultId">The ID of the vault that contains the item to retrieve.</param>
    /// <returns>The item details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item GetItem(string itemId, string vaultId);

    /// <summary>Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="vault">The vault that contains the item to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <returns>The item that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item SearchForItem(IItem item, IVault? vault = null, bool? includeArchive = null);

    /// <summary>Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="itemId">The ID of the item to search for.</param>
    /// <param name="vaultId">The ID of the vault that contains the item to search for.</param>
    /// <param name="includeArchive">When <see langword="true" />, includes archived items in the search.</param>
    /// <returns>The item that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item SearchForItem(string itemId, string? vaultId = null, bool? includeArchive = null);

    /// <summary>Creates an item.</summary>
    /// <param name="template">The template from which to create the item.</param>
    /// <param name="vault">The vault in which to create the item.</param>
    /// <returns>The created item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item CreateItem(Template template, IVault vault);

    /// <summary>Creates an item.</summary>
    /// <param name="template">The template from which to create the item.</param>
    /// <param name="vaultId">The ID of the vault in which to create the item.</param>
    /// <returns>The created item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item CreateItem(Template template, string vaultId);

    /// <summary>Edits an item.</summary>
    /// <param name="item">The item to edit.</param>
    /// <param name="vault">The vault that contains the item to edit.</param>
    /// <returns>The edited item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item EditItem(Item item, IVault vault);

    /// <summary>Edits an item.</summary>
    /// <param name="item">The item to edit.</param>
    /// <param name="vaultId">The ID of the vault that contains the item to edit.</param>
    /// <returns>The edited item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item EditItem(Item item, string vaultId);

    /// <summary>Archives an item.</summary>
    /// <param name="item">The item to archive.</param>
    /// <param name="vault">The vault that contains the item to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveItem(IItem item, IVault vault);

    /// <summary>Archives an item.</summary>
    /// <param name="itemId">The ID of the item to archive.</param>
    /// <param name="vaultId">The ID of the vault that contains the item to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveItem(string itemId, string vaultId);

    /// <summary>Deletes an item.</summary>
    /// <param name="item">The item to delete.</param>
    /// <param name="vault">The vault that contains the item to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteItem(IItem item, IVault vault);

    /// <summary>Deletes an item.</summary>
    /// <param name="itemId">The ID of the item to delete.</param>
    /// <param name="vaultId">The ID of the vault that contains the item to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteItem(string itemId, string vaultId);
}