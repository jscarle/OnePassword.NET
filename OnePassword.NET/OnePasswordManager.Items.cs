using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly string[] AssignmentChars = { ".", "=", "[", "]", "\\", "\"" };

    /// <summary>
    /// Gets a vault's items.
    /// </summary>
    /// <param name="vault">The vault that contains the items to retrieve.</param>
    /// <returns>The vault's items.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public ImmutableList<Item> GetItems(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item list --vault {vault.Id}";
        return Op<ImmutableList<Item>>(command);
    }

    /// <summary>
    /// Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="vault">The vault that contains the items to search for.</param>
    /// <param name="includeArchive">When <see langword="true"/>, includes archived items in the search.</param>
    /// <param name="favorite">When <see langword="true"/>, searches for favorites.</param>
    /// <param name="categories">The categories to search for.</param>
    /// <param name="tags">The tags to search for.</param>
    /// <returns>The items that match the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Gets an item.
    /// </summary>
    /// <param name="item">The item to retrieve.</param>
    /// <param name="vault">The vault that contains the item to retrieve.</param>
    /// <returns>The item details.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item GetItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item get {item.Id} --vault {vault.Id}";
        return Op<Item>(command);
    }

    /// <summary>
    /// Searches for an item.
    /// <remarks>WARNING: If a vault is not specified, the 1Password CLI may generate a large amount of internal calls which may result in throttling.</remarks>
    /// </summary>
    /// <param name="item">The item to search for.</param>
    /// <param name="vault">The vault that contains the item to search for.</param>
    /// <param name="includeArchive">When <see langword="true"/>, includes archived items in the search.</param>
    /// <returns>The item that matches the search.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
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

    /// <summary>
    /// Creates an item.
    /// </summary>
    /// <param name="template">The template from which to create the item.</param>
    /// <param name="vault">The vault in which to create the item.</param>
    /// <returns>The created item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item CreateItem(Template template, IVault vault)
    {
        if (vault == null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var json = JsonSerializer.Serialize(template) + "\x04";

        var command = $"item create --vault {vault.Id} -";
        ((ITracked)template).AcceptChanges();
        return Op<Item>(command, json);
    }

    /// <summary>
    /// Edits an item.
    /// </summary>
    /// <param name="item">The item to edit.</param>
    /// <param name="vault">The vault that contains the item to edit.</param>
    /// <returns>The edited item.</returns>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public Item EditItem(Item item, IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item edit {item.Id}";
        if (((ITracked)item.Fields).Changed)
        {
            command = item.Fields
                .Where(field => ((ITracked)field).Changed)
                .Aggregate(command, (current, field) => current + GetFieldAssignment(field));
            command = item.Fields.Removed
                .Aggregate(command, (current, field) => current + GetFieldAssignment(field, true));
        }
        command += $" --vault {vault.Id}";
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
        return Op<Item>(command);
    }

    /// <summary>
    /// Archives an item.
    /// </summary>
    /// <param name="item">The item to archive.</param>
    /// <param name="vault">The vault that contains the item to archive.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void ArchiveItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item delete {item.Id} --vault {vault.Id} --archive";
        Op(command);
    }

    /// <summary>
    /// Deletes an item.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    /// <param name="vault">The vault that contains the item to delete.</param>
    /// <exception cref="ArgumentException">Thrown when there is an invalid argument.</exception>
    public void DeleteItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item delete {item.Id} --vault {vault.Id}";
        Op(command);
    }

    private static string GetFieldAssignment(Field field, bool delete = false)
    {
        var fieldAssignment = " \"";
        if (field.Section is not null)
            fieldAssignment += $"{EscapeLabel(field.Section.Label)}.";
        fieldAssignment += EscapeLabel(field.Label);
        if (delete)
        {
            fieldAssignment += "[delete]";
        }
        else
        {
            if (field.TypeChanged)
                fieldAssignment += $"[{field.Type.ToEnumString().ToLower().Replace(" ", "", StringComparison.InvariantCulture)}]";
            fieldAssignment += $"={field.Value}";
        }
        fieldAssignment += "\"";
        return fieldAssignment;
    }

    private static string EscapeLabel(string label)
    {
        return AssignmentChars.Aggregate(label, (current, assignmentChar) => current.Replace(assignmentChar, $@"\{assignmentChar}"));
    }
}