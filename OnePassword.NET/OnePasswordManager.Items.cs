using OnePassword.Common;
using OnePassword.Items;
using OnePassword.Templates;
using OnePassword.Vaults;

namespace OnePassword;

public sealed partial class OnePasswordManager
{
    private static readonly string[] AssignmentChars = { ".", "=", "[", "]", "\\", "\"" };

    /// <inheritdoc />
    public ImmutableList<Item> GetItems(IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item list --vault {vault.Id}";
        return Op<ImmutableList<Item>>(command);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Item GetItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item get {item.Id} --vault {vault.Id}";
        return Op<Item>(command);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public Item CreateItem(Template template, IVault vault)
    {
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var json = JsonSerializer.Serialize(template) + "\x04";

        var command = $"item create - --vault {vault.Id}";
        ((ITracked)template).AcceptChanges();
        return Op<Item>(command, json);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public void ArchiveItem(IItem item, IVault vault)
    {
        if (item.Id is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        var command = $"item delete {item.Id} --vault {vault.Id} --archive";
        Op(command);
    }

    /// <inheritdoc />
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