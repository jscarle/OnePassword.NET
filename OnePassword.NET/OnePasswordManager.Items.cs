using System.Collections.Immutable;
using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    public ImmutableList<Item> GetItems(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item list --vault {vault.Id}";
        return Op<ImmutableList<Item>>(command);
    }

    public ImmutableList<Item> SearchForItems(IVault? vault = null, bool? includeArchive = null, bool? favorite = null, IReadOnlyCollection<Category>? categories = null, IReadOnlyCollection<string>? tags = null)
    {
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = "item list";
        if (vault is not null)
            command += $" --vault {vault.Id}";
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

    public Item GetItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item get {item.Id} --vault {vault.Id}";
        return Op<Item>(command);
    }

    public Item SearchForItem(IItem item, IVault? vault = null, bool? includeArchive = null)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is not null && vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item get {item.Id}";
        if (vault is not null)
            command += $" --vault {vault.Id}";
        if (includeArchive is not null && includeArchive.Value)
            command += " --include-archive";
        return Op<Item>(command);
    }

    public Item CreateItem(Item item, IVault vault)
    {
        if (item.Id is not null)
            throw new ArgumentException($"{nameof(item.Id)} must be null.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var json = JsonSerializer.Serialize(item) + "\x04";

        var command = $"item create - --vault {vault.Id}";
        return Op<Item>(command, json);
    }

    public void ArchiveItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item delete {item.Id} --vault {vault.Id} --archive";
        Op(command);
    }

    public void DeleteItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item delete {item.Id} --vault {vault.Id}";
        Op(command);
    }
}