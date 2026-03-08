using System.Text.Json;
using System.Globalization;
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
        return Op(JsonContext.Default.ImmutableListItem, command);
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
        return Op(JsonContext.Default.ImmutableListItem, command);
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
        return Op(JsonContext.Default.Item, command);
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
        return Op(JsonContext.Default.Item, command);
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

        var json = JsonSerializer.Serialize(template, JsonContext.Default.Template) + "\x04";

        var command = $"item create --vault {vaultId} -";
        ((ITracked)template).AcceptChanges();
        if (template.TitleChanged)
            command += $" --title \"{template.Title}\"";
        if (((ITracked)template.Tags).Changed)
            command += $" --tags \"{template.Tags.ToCommaSeparated()}\"";
        return Op(JsonContext.Default.Item, command, json);
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

        var json = JsonSerializer.Serialize(item
#if NET7_0_OR_GREATER
                , JsonContext.Default.Item
#endif
            )
            + "\x04";

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
        return Op(JsonContext.Default.Item, command, json);
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

    /// <inheritdoc />
    public void MoveItem(IItem item, IVault currentVault, IVault destinationVault)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (currentVault is null || currentVault.Id.Length == 0)
            throw new ArgumentException($"{nameof(currentVault.Id)} cannot be empty.", nameof(currentVault));
        if (destinationVault is null || destinationVault.Id.Length == 0)
            throw new ArgumentException($"{nameof(destinationVault.Id)} cannot be empty.", nameof(destinationVault));

        MoveItem(item.Id, currentVault.Id, destinationVault.Id);
    }

    /// <inheritdoc />
    public void MoveItem(string itemId, string currentVaultId, string destinationVaultId)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (currentVaultId is null || currentVaultId.Length == 0)
            throw new ArgumentException($"{nameof(currentVaultId)} cannot be empty.", nameof(currentVaultId));
        if (destinationVaultId is null || destinationVaultId.Length == 0)
            throw new ArgumentException($"{nameof(destinationVaultId)} cannot be empty.", nameof(destinationVaultId));

        var command = $"item move {itemId} --current-vault {{currentVaultId}} --destination-vault {{destinationVaultId}}";
        Op(command);
    }

    /// <inheritdoc />
    public ItemShare ShareItem(IItem item, IVault vault, string emailAddress, TimeSpan? expiresIn = null, bool? viewOnce = null)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));
        if (emailAddress is null || emailAddress.Length == 0)
            throw new ArgumentException($"{nameof(emailAddress)} cannot be empty.", nameof(emailAddress));
        var trimmedEmailAddress = emailAddress.Trim();
        if (trimmedEmailAddress.Length == 0)
            throw new ArgumentException($"{nameof(trimmedEmailAddress)} cannot be empty.", nameof(emailAddress));

        return ShareItem(item.Id, vault.Id, trimmedEmailAddress, expiresIn, viewOnce);
    }

    /// <inheritdoc />
    public ItemShare ShareItem(string itemId, string vaultId, string emailAddress, TimeSpan? expiresIn = null, bool? viewOnce = null)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));
        if (emailAddress is null || emailAddress.Length == 0)
            throw new ArgumentException($"{nameof(emailAddress)} cannot be empty.", nameof(emailAddress));
        var trimmedEmailAddress = emailAddress.Trim();
        if (trimmedEmailAddress.Length == 0)
            throw new ArgumentException($"{nameof(trimmedEmailAddress)} cannot be empty.", nameof(emailAddress));

        return ShareItem(itemId, vaultId, new[] { trimmedEmailAddress }, expiresIn, viewOnce);
    }

    /// <inheritdoc />
    public ItemShare ShareItem(IItem item, IVault vault, IReadOnlyCollection<string>? emailAddresses = null, TimeSpan? expiresIn = null, bool? viewOnce = null)
    {
        if (item is null || item.Id.Length == 0)
            throw new ArgumentException($"{nameof(item.Id)} cannot be empty.", nameof(item));
        if (vault is null || vault.Id.Length == 0)
            throw new ArgumentException($"{nameof(vault.Id)} cannot be empty.", nameof(vault));

        return ShareItem(item.Id, vault.Id, emailAddresses, expiresIn, viewOnce);
    }

    /// <inheritdoc />
    public ItemShare ShareItem(string itemId, string vaultId, IReadOnlyCollection<string>? emailAddresses = null, TimeSpan? expiresIn = null, bool? viewOnce = null)
    {
        if (itemId is null || itemId.Length == 0)
            throw new ArgumentException($"{nameof(itemId)} cannot be empty.", nameof(itemId));
        if (vaultId is null || vaultId.Length == 0)
            throw new ArgumentException($"{nameof(vaultId)} cannot be empty.", nameof(vaultId));

        var command = $"item share {itemId} --vault {vaultId}";
        var normalizedEmailAddresses = NormalizeEmailAddresses(emailAddresses);
        if (normalizedEmailAddresses.Count > 0)
            command += $" --emails {string.Join(',', normalizedEmailAddresses)}";
        if (expiresIn is not null)
            command += $" --expires-in {expiresIn.Value.ToHumanReadable()}";
        if (viewOnce is not null && viewOnce.Value)
            command += " --view-once";
        return ParseItemShare(Op(command));
    }

    private static DateTimeOffset? GetDateTimeOffsetProperty(JsonElement root, params string[] propertyNames)
    {
        var stringValue = GetStringProperty(root, propertyNames);
        return DateTimeOffset.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTimeOffset) ? dateTimeOffset : null;
    }

    private static Uri? GetUriProperty(JsonElement root, params string[] propertyNames)
    {
        var stringValue = GetStringProperty(root, propertyNames);
        return Uri.TryCreate(stringValue, UriKind.Absolute, out var uri) ? uri : null;
    }

    private static bool? GetBooleanProperty(JsonElement root, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!root.TryGetProperty(propertyName, out var property))
                continue;

            return property.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.String when bool.TryParse(property.GetString(), out var value) => value,
                _ => null
            };
        }

        return null;
    }

    private static string? GetStringProperty(JsonElement root, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!root.TryGetProperty(propertyName, out var property))
                continue;

            if (property.ValueKind == JsonValueKind.String)
                return property.GetString();
        }

        return null;
    }

    private static ImmutableList<string> GetRecipients(JsonElement root, params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            if (!root.TryGetProperty(propertyName, out var property))
                continue;

            return property.ValueKind switch
            {
                JsonValueKind.String => property.GetString() is { Length: > 0 } recipient ? [recipient] : [],
                JsonValueKind.Array => [.. property
                    .EnumerateArray()
                    .Select(static recipient => recipient.ValueKind switch
                    {
                        JsonValueKind.String => recipient.GetString(),
                        JsonValueKind.Object => GetStringProperty(recipient, "email", "address", "recipient", "value"),
                        _ => null
                    })
                    .Where(static recipient => !string.IsNullOrWhiteSpace(recipient))
                    .Select(static recipient => recipient!)],
                _ => []
            };
        }

        return [];
    }

    private static ImmutableList<string> NormalizeEmailAddresses(IReadOnlyCollection<string>? emailAddresses)
    {
        if (emailAddresses is null || emailAddresses.Count == 0)
            return [];

        return [.. emailAddresses
            .Where(static emailAddress => !string.IsNullOrWhiteSpace(emailAddress))
            .Select(static emailAddress => emailAddress.Trim())];
    }

    private static ItemShare ParseItemShare(string result)
    {
        var trimmedResult = result.Trim();
        if (trimmedResult.Length == 0)
            return new ItemShare();

        try
        {
            using var jsonDocument = JsonDocument.Parse(trimmedResult);
            var root = jsonDocument.RootElement;

            if (root.ValueKind == JsonValueKind.String)
            {
                return new ItemShare
                {
                    Url = Uri.TryCreate(root.GetString(), UriKind.Absolute, out var uri) ? uri : null
                };
            }

            if (root.ValueKind != JsonValueKind.Object)
                return new ItemShare();

            return new ItemShare
            {
                Url = GetUriProperty(root, "url", "link", "share_link", "shareLink"),
                ExpiresAt = GetDateTimeOffsetProperty(root, "expires_at", "expiresAt", "expiry", "expires"),
                Recipients = GetRecipients(root, "recipients", "emails", "email_addresses", "emailAddresses"),
                ViewOnce = GetBooleanProperty(root, "view_once", "viewOnce")
            };
        }
        catch (JsonException)
        {
            return new ItemShare
            {
                Url = Uri.TryCreate(trimmedResult, UriKind.Absolute, out var uri) ? uri : null
            };
        }
    }
}
