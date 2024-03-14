using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    /// <inheritdoc />
    public ImmutableList<Item> GetItems(IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetItems(vault.Id);
    }

    /// <inheritdoc />
    public ImmutableList<Item> GetItems(string vaultId)
    {
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item list --vault {vaultId}";
        return Op<ImmutableList<Item>>(command);
    }

    /// <inheritdoc />
    public ImmutableList<Item> SearchForItems(IVault vault, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null, IReadOnlyCollection<string>? tags = null)
    {
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return SearchForItems(vault?.Id, includeArchive, favorite, categories, tags);
    }

    /// <inheritdoc />
    public ImmutableList<Item> SearchForItems(string? vaultId = null, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null,
        IReadOnlyCollection<string>? tags = null)
    {
        if (vaultId is not null && vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = "item list";
        if (vaultId is not null)
            command += $" --vault {vaultId}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        if (favorite is not null && favorite.Value)
            command += " --favorite";
        if (categories is not null && categories.Count > 0)
            command += $" --categories \"{categories.ToCommaSeparated(true)}\"";
        if (tags is not null && tags.Count > 0)
            command += $" --tags \"{tags.ToCommaSeparated()}\"";
        return Op<ImmutableList<Item>>(command);
    }

    /// <inheritdoc />
    public Item GetItem(IItem item, IVault vault)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return GetItem(item.Id, vault.Id);
    }

    /// <inheritdoc />
    public Item GetItem(string itemId, string vaultId)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item get {itemId} --vault {vaultId}";
        return Op<Item>(command);
    }

    /// <inheritdoc />
    public Item SearchForItem(IItem item, IVault? vault = null, bool? includeArchive = null)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return SearchForItem(item.Id, vault?.Id, includeArchive);
    }

    /// <inheritdoc />
    public Item SearchForItem(string itemId, string? vaultId = null, bool? includeArchive = null)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is not null && vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item get {itemId}";
        if (vaultId is not null)
            command += $" --vault {vaultId}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        return Op<Item>(command);
    }

    /// <inheritdoc />
    public Item CreateItem(Template template, IVault vault)
    {
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return CreateItem(template, vault.Id);
    }

    /// <inheritdoc />
    public Item CreateItem(Template template, string vaultId)
    {
        if (template is null)
            throw new ArgumentException($"{nameof(template)} cannot be empty.", nameof(template));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var json = JsonSerializer.Serialize(template) + "\x04";

        var command = $"item create --vault {vaultId} -";
        ((ITracked)template).AcceptChanges();
        if (template.TitleChanged)
            command += $" --title \"{template.Title}\"";
        if (((ITracked)template.Tags).Changed)
            command += $" --tags \"{template.Tags.ToCommaSeparated()}\"";
        return Op<Item>(command, json);
    }

    /// <inheritdoc />
    public Item EditItem(Item item, IVault vault)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return EditItem(item, vault.Id);
    }

    /// <inheritdoc />
    public Item EditItem(Item item, string vaultId)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var json = JsonSerializer.Serialize(item) + "\x04";

        var command = $"item edit {item.Id} --vault {vaultId}";
        if (item.TitleChanged)
            command += $" --title \"{item.Title}\"";
        if (((ITracked)item.Tags).Changed)
            command += $" --tags \"{item.Tags.ToCommaSeparated()}\"";
        if (((ITracked)item.Urls).Changed)
        {
            var changedUrl = item.Urls.FirstOrDefault(url => url.Primary && ((ITracked)url).Changed);
            command += $" --url \"{changedUrl}\"";
        }
        ((ITracked)item).AcceptChanges();
        return Op<Item>(command, json);
    }

    /// <inheritdoc />
    public void ArchiveItem(IItem item, IVault vault)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        ArchiveItem(item.Id, vault.Id);
    }

    /// <inheritdoc />
    public void ArchiveItem(string itemId, string vaultId)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item delete {itemId} --vault {vaultId} --archive";
        Op(command);
    }

    /// <inheritdoc />
    public void DeleteItem(IItem item, IVault vault)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        DeleteItem(item.Id, vault.Id);
    }

    /// <inheritdoc />
    public void DeleteItem(string itemId, string vaultId)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item delete {itemId} --vault {vaultId}";
        Op(command);
    }
}